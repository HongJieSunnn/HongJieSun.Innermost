using System.Text;

namespace Innermost.Meet.SignalRHub.Infrastructure.Services
{
    public class ChattingRecordRedisService : IChattingRecordRedisService
    {
        private readonly MeetRedisContext _redis;
        private readonly IMediator _mediator;
        private readonly ILogger<ChattingRecordRedisService> _logger;
        public ChattingRecordRedisService(MeetRedisContext redisContext, IMediator mediator, ILogger<ChattingRecordRedisService> logger)
        {
            _redis = redisContext;
            _mediator = mediator;
            _logger = logger;
        }
        public async Task<long> AddChattingRecordAsync(string chattingContextId, string chattingRecord)
        {
            return await _redis.Context().ListRightPushAsync(chattingContextId, chattingRecord);
        }

        public Task<long> AddChattingRecordAsync(string chattingContextId, string sendUserId, string message, bool received)
        {
            var chattingRecordId = ObjectId.GenerateNewId().ToString();
            var chattingRecordDTO = new ChattingRecordDTO(chattingRecordId, sendUserId, message, DateTime.Now, null);

            var chattingRecordJsonString = $"{JsonSerializer.Serialize(chattingRecordDTO)}{(received ? 1 : 0)}";

            return AddChattingRecordAsync(chattingContextId, chattingRecordJsonString);
        }

        public Task<long> AddChattingRecordAsync(string chattingContextId, ChattingRecordDTO chattingRecordDTO,bool received)
        {
            if (chattingRecordDTO.ChattingRecordId is null)
                throw new ArgumentException("ChattingRecordDTO.ChattingRecordId must not be null while call method AddChattingRecordAsync(string chattingContextId, ChattingRecordDTO chattingRecordDTO)");

            var chattingRecordJsonString = $"{JsonSerializer.Serialize(chattingRecordDTO)}{(received ? 1 : 0)}";

            return AddChattingRecordAsync(chattingContextId, chattingRecordJsonString);
        }

        public async Task<IEnumerable<ChattingRecordDTO>> GetAllChattingRecordsAsync(string chattingContextId)
        {
            var listLength = await _redis.Context().ListLengthAsync(chattingContextId);

            var chattingRecordJsonStrings = await _redis.Context().ListRangeAsync(chattingContextId, 0, listLength);

            var chattingRecords = chattingRecordJsonStrings.Select(rv => JsonSerializer.Deserialize<ChattingRecordDTO>(rv)!);

            return chattingRecords ?? new List<ChattingRecordDTO>();
        }

        public async Task<IEnumerable<ChattingRecordDTO>> GetNotReceivedChattingRecordsAsync(string chattingContextId)
        {
            var listLength = await _redis.Context().ListLengthAsync(chattingContextId);
            //list is order by CreateTime which means the right side chatting record is later record.
            //We ergodic from right side,if the first element in right side has been received,all elements have been received.
            var ans = new List<ChattingRecordDTO>();
            for (long i = listLength - 1; i >= 0; --i)
            {
                var chattingRecordString = GetChattingRecordStringFromRedisByIndex(chattingContextId, i);

                if (chattingRecordString is not null && chattingRecordString[chattingRecordString.Length - 1] == '1')
                {
                    break;//the chattingRecords before a received chattingRecord are received.
                }
                else if (chattingRecordString is null)
                {
                    throw new NullReferenceException($"Get ChattingRecord from redis failed");
                }

                ans.Add(DeserializeChattingRecordDTOByChattingRecordStringFromRedis(chattingRecordString));
            }

            ans.Reverse();

            return ans;
        }

        public async Task SetNotReceivedChattingRecordsReceivedAsync(string chattingContextId, int modifyCount)
        {
            RedisValue[] chattingRecordArr = new RedisValue[modifyCount];
            for (int i = 0; i < modifyCount; i++)
            {
                var chattingRecordString = new StringBuilder(await _redis.Context().ListRightPopAsync(chattingContextId));

                chattingRecordString[chattingRecordString.Length - 1] = '1';

                chattingRecordArr[modifyCount - i - 1] = chattingRecordString.ToString();
            }

            await _redis.Context().ListRightPushAsync(chattingContextId, chattingRecordArr);
        }

        public async Task<IEnumerable<ChattingRecordDTO>> GetNotReceivedChattingRecordsAndSetAsReceivedAsync(string chattingContextId)
        {
            var listLength = await _redis.Context().ListLengthAsync(chattingContextId);

            var ans = new List<ChattingRecordDTO>();
            for (long i = listLength - 1; i >= 0; --i)
            {
                var chattingRecordString = GetChattingRecordStringFromRedisByIndex(chattingContextId, i);

                if (chattingRecordString is not null && chattingRecordString[chattingRecordString.Length - 1] == '1')
                {
                    break;
                }
                else if (chattingRecordString is null)
                {
                    throw new NullReferenceException($"Get ChattingRecord from redis failed");
                }

                ans.Add(DeserializeChattingRecordDTOByChattingRecordStringFromRedis(chattingRecordString));
            }

            await SetNotReceivedChattingRecordsReceivedAsync(chattingContextId, ans.Count);
            ans.Reverse();

            return ans;
        }

        private StringBuilder GetChattingRecordStringFromRedisByIndex(string chattingContextId, long index)
        {
            return new StringBuilder(_redis.Context().ListGetByIndex(chattingContextId, index).ToString());
        }

        private ChattingRecordDTO DeserializeChattingRecordDTOByChattingRecordStringFromRedis(StringBuilder redisChattingRecordString)
        {
            redisChattingRecordString.Remove(redisChattingRecordString.Length - 1, 1);

            var chattingRecordDTO = JsonSerializer.Deserialize<ChattingRecordDTO>(redisChattingRecordString.ToString());

            return chattingRecordDTO ?? throw new InvalidOperationException($"Can not deserialize chattingRecordString ({redisChattingRecordString}) to ChattingRecordDTO.");
        }

        public async Task PersistReceivedChattingRecordToMongoDBAsync(string chattingContextId)
        {
            List<ChattingRecordDTO> chattingRecordDTOList = new List<ChattingRecordDTO>();
            RedisValue chattingRecordRedisValue = RedisValue.Null;
            while (true)
            {
                chattingRecordRedisValue = await _redis.Context().ListLeftPopAsync(chattingContextId);

                if (chattingRecordRedisValue == RedisValue.Null)//List empty
                    break;

                var chattingRecordStringBuilder = new StringBuilder(chattingRecordRedisValue);

                if (chattingRecordStringBuilder[chattingRecordStringBuilder.Length - 1] == '0')//Not received.
                    break;

                chattingRecordDTOList.Add(DeserializeChattingRecordDTOByChattingRecordStringFromRedis(chattingRecordStringBuilder));
            }

            var persistCommand = new PersistReceivedChattingRecordToMongoDBCommand(chattingContextId, chattingRecordDTOList);

            _logger.LogSendCommand(Guid.NewGuid().ToString(), nameof(PersistReceivedChattingRecordToMongoDBCommand), nameof(persistCommand.ChattingContextId), persistCommand.ChattingContextId, persistCommand);

            await _mediator.Send(persistCommand);
        }
    }
}
