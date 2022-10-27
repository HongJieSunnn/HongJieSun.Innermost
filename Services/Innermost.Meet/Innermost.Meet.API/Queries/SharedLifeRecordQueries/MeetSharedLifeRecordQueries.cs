using System.Linq.Expressions;

namespace Innermost.Meet.API.Queries.SharedLifeRecordQueries
{
    public class MeetSharedLifeRecordQueries : IMeetSharedLifeRecordQueries
    {
        private readonly MeetMongoDBContext _context;
        private readonly IIdentityProfileService _identityService;
        public MeetSharedLifeRecordQueries(MeetMongoDBContext context, IIdentityProfileService identityService)
        {
            _context = context;
            _identityService = identityService;
        }
        public async Task<IEnumerable<SharedLifeRecordDTO>> GetRandomSharedLifeRecordsAsync(int limit = 20, Expression<Func<SharedLifeRecord, bool>>? filter = null)
        {
            var userId = _identityService.GetUserId();

            List<SharedLifeRecord> records;

            if (filter is null)
                records = await _context.SharedLifeRecords.AsQueryable().Where(l => l.UserId != userId).Sample(limit).ToListAsync();//Method Sample(in MongoDD.Driver.Linq) is to get random limit count records.
            else
                records = await _context.SharedLifeRecords.AsQueryable().Where(l => l.UserId != userId).Where(filter).Sample(limit).ToListAsync();

            return records.Select(r => new SharedLifeRecordDTO(r));
        }

        public async Task<IEnumerable<SharedLifeRecordDTO>> GetSharedLifeRecordsByLocationAsync(float longitude, float latitude, int page = 1, int limit = 20, double minDistance = 0, double maxDistance = 5000, string sortBy = "Id")
        {
            var userId = _identityService.GetUserId();

            var sortByFilter = Builders<SharedLifeRecord>.Sort.Descending(sortBy);//SortBy id,createTime or likesCount.Only descending sort(recent records or most likescount records).

            var nearFilter = Builders<SharedLifeRecord>.Filter.Ne(l => l.UserId, userId) &
                                Builders<SharedLifeRecord>.Filter.Ne(l => l.Location, null) &
                                Builders<SharedLifeRecord>.Filter.Near(l => l.Location!.BaiduPOI, latitude, longitude, maxDistance, minDistance);

            var records = await _context.SharedLifeRecords.Find(nearFilter).Sort(sortByFilter).Skip((page - 1) * limit).Limit(limit).ToListAsync();

            return records.Select(r => new SharedLifeRecordDTO(r));
        }

        public async Task<IEnumerable<SharedLifeRecordDTO>> GetSharedLifeRecordsByMusicRecordAsync(string musicRecordMid, int page = 1, int limit = 20, string sortBy = "Id")
        {
            var userId = _identityService.GetUserId();

            var sortByFilter = Builders<SharedLifeRecord>.Sort.Descending(sortBy);

            var filter = Builders<SharedLifeRecord>.Filter.Ne(l => l.MusicRecord, null) &
                            Builders<SharedLifeRecord>.Filter.Eq(l => l.MusicRecord!.MusicMid, musicRecordMid);

            var records = await _context.SharedLifeRecords.Find(filter).Sort(sortByFilter).Skip((page - 1) * limit).Limit(limit).ToListAsync();

            return records.Select(r => new SharedLifeRecordDTO(r));
        }

        public async Task<IEnumerable<SharedLifeRecordDTO>> GetSharedLifeRecordsByTagsAsync(IEnumerable<string> tagIds, int page = 1, int limit = 20, string sortBy = "Id")
        {
            var userId = _identityService.GetUserId();

            var sortByFilter = Builders<SharedLifeRecord>.Sort.Descending(sortBy);

            var filter = Builders<SharedLifeRecord>.Filter.Ne(l => l.UserId, userId) &
                            Builders<SharedLifeRecord>.Filter.ElemMatch(l => l.Tags, Builders<TagSummary>.Filter.AnyIn("TagId", tagIds));

            var records = await _context.SharedLifeRecords.Find(filter).Sort(sortByFilter).Skip((page - 1) * limit).Limit(limit).ToListAsync();

            return records.Select(r => new SharedLifeRecordDTO(r));
        }
    }
}
