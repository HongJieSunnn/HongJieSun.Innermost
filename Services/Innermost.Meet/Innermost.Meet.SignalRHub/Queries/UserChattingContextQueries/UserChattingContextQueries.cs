using Innermost.Meet.Domain.AggregatesModels.UserChattingAggregate;
using Innermost.Meet.Domain.AggregatesModels.UserChattingAggregate.Entities;

namespace Innermost.Meet.SignalRHub.Queries.UserChattingContextQueries
{
    public class UserChattingContextQueries : IUserChattingContextQueries
    {
        private readonly MeetMongoDBContext _context;
        private readonly IChattingRecordRedisService _chattingRecordRedisService;
        public UserChattingContextQueries(MeetMongoDBContext context,IChattingRecordRedisService chattingRecordRedisService)
        {
            _context=context;
            _chattingRecordRedisService=chattingRecordRedisService;
        }
        public async Task<IEnumerable<string>> GetAllChattingContextIdsOfUserAsync(string userId)
        {
            var filter = Builders<UserChattingContext>.Filter.Eq("Users", userId);

            var userChattingContexts = await _context.UserChattingContexts.Find(filter).ToListAsync();

            return userChattingContexts.Select(ucc => ucc.Id!);
        }

        public async Task<string> GetChattingContextIdOfUsers(string userId1, string userId2)
        {
            var users = new[] { userId1, userId2 }.OrderBy(s => s).ToArray();

            var filter = Builders<UserChattingContext>.Filter.Eq(ucc => ucc.Users, users);
            var chattingContextIdProjection=Builders<UserChattingContext>.Projection.Include("_id");//To avoid take chattingRecords which may be very large.

            var chattingContext =await _context.UserChattingContexts.Find(filter).Project(chattingContextIdProjection).FirstAsync();

            return chattingContext["_id"].ToString()!;
        }

        public async Task<IEnumerable<ChattingRecordDTO>> GetChattingRecordsAsync(string chattingContextId, int page = 1, int limit = 50)
        {
            var ans = new List<ChattingRecordDTO>(limit);
            var chattingRecordsInRedis=await _chattingRecordRedisService.GetAllChattingRecordsAsync(chattingContextId);

            if(chattingRecordsInRedis.Any())
            {
                ans.AddRange(chattingRecordsInRedis.Skip((page-1) * limit).Take(limit));

                if (ans.Count == limit)
                    return ans;
            }

            //ans.Count==0 means that didn't take any records in redis and which means (page-1)*limit is larger than count of records in redis.
            //So,has skiped for records' count in redis,we need to start skip in mongodb by (page-1)*limit - records' count in redis.
            //ans.Count==1 means that take some records in redis,so we should not skip in mongodb.
            var skipForMongoDB = ans.Count == 0 ? ((page - 1) * limit - chattingContextId.Length) : 0;
            var limitForMongoDB = limit - ans.Count;

            var filter = Builders<UserChattingContext>.Filter.Eq(ucc => ucc.Id, chattingContextId);

            //Slice:https://www.mongodb.com/docs/manual/reference/operator/aggregation/slice/#mongodb-expression-exp.-slice
            //Projections:https://mongodb.github.io/mongo-csharp-driver/2.14/reference/driver/definitions/
            var projection =Builders<UserChattingContext>.Projection.Slice(ucc=>ucc.ChattingRecords, skipForMongoDB,limitForMongoDB);

            var chattingRecordInMongoDB = await _context.UserChattingContexts.Find(filter).Project<UserChattingContext>(projection).FirstAsync();

            ans.AddRange(chattingRecordInMongoDB.ChattingRecords.Select(cr=>MapToChattingRecordDTO(cr)));

            return ans;
        }

        private ChattingRecordDTO MapToChattingRecordDTO(ChattingRecord chattingRecord)
        {
            return new ChattingRecordDTO(
                chattingRecord.Id!,
                chattingRecord.SendUserId,
                chattingRecord.RecordMessage,
                chattingRecord.CreateTime,
                chattingRecord.Tags.Select(tsdto => new TagSummaryDTO(tsdto.TagId, tsdto.TagName)).ToList()
            );
        }
    }
}
