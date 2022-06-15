namespace Innermost.Meet.API.Queries.SharedLifeRecordQueries
{
    public class OwnSharedLifeRecordQueries : IOwnSharedLifeRecordQueries
    {
        private readonly MeetMongoDBContext _context;
        private readonly IUserIdentityService _identityService;
        public OwnSharedLifeRecordQueries(MeetMongoDBContext context, IUserIdentityService identityService)
        {
            _context = context;
            _identityService = identityService;
        }

        public async Task<IEnumerable<SharedLifeRecordDTO>> GetAllOwnSharedLifeRecordsAsync()
        {
            var userId = _identityService.GetUserId();

            var records = await _context.SharedLifeRecords.Find(t => t.UserId == userId).ToListAsync();

            return records.Select(r => new SharedLifeRecordDTO(r));
        }

        public async Task<IEnumerable<SharedLifeRecordDTO>> GetAllOwnSharedLifeRecordsByTimeAsync(DateTimeToFind dateTimeToFind)
        {
            var userId = _identityService.GetUserId();
            var dateTimeToFindPair = dateTimeToFind.GetStartAndEndTimePair();

            var filter = Builders<SharedLifeRecord>.Filter.Eq(l => l.UserId, userId) &
                            Builders<SharedLifeRecord>.Filter.Gte(l => l.CreateTime, dateTimeToFindPair.startTime) &
                            Builders<SharedLifeRecord>.Filter.Lte(l => l.CreateTime, dateTimeToFindPair.endTime);
            var records = await _context.SharedLifeRecords.Find(filter).ToListAsync();

            return records.Select(r => new SharedLifeRecordDTO(r));
        }

        public async Task<IEnumerable<SharedLifeRecordDTO>> GetOwnSharedLifeRecordsAsync(int page = 1, int limit = 20)
        {
            var userId = _identityService.GetUserId();

            var records = await _context.SharedLifeRecords.Find(t => t.UserId == userId).Skip((page - 1) * limit).Limit(limit).ToListAsync();

            return records.Select(r => MapToSharedLifeRecordDTO(r));
        }

        private SharedLifeRecordDTO MapToSharedLifeRecordDTO(SharedLifeRecord sharedLifeRecord)
        {
            return new SharedLifeRecordDTO(
                  sharedLifeRecord.Id!, sharedLifeRecord.RecordId, 
                  sharedLifeRecord.UserId,sharedLifeRecord.UserName,sharedLifeRecord.UserNickName,sharedLifeRecord.UserAvatarUrl, 
                  sharedLifeRecord.Title, sharedLifeRecord.Text,

                  (sharedLifeRecord.Location is null) ? null : new LocationDTO(sharedLifeRecord.Location.Id!,
                      sharedLifeRecord.Location.LocationName, sharedLifeRecord.Location.Province, sharedLifeRecord.Location.City, sharedLifeRecord.Location.District, sharedLifeRecord.Location.Address,
                      (float)sharedLifeRecord.Location.BaiduPOI.Coordinates.Longitude, (float)sharedLifeRecord.Location.BaiduPOI.Coordinates.Latitude),

                  (sharedLifeRecord.MusicRecord is null) ? null : new MusicRecordDTO(sharedLifeRecord.MusicRecord.Id!, sharedLifeRecord.MusicRecord.MusicName, sharedLifeRecord.MusicRecord.Singer, sharedLifeRecord.MusicRecord.Album),

                  sharedLifeRecord.ImagePaths?.ToList(),

                  sharedLifeRecord.LikesCount,
                  sharedLifeRecord.Likes.Select(l => new LikeDTO(l.LikerUserId, l.LikerUserName,l.LikerUserNickName, l.LikerUserAvatarUrl, l.LikeTime)).ToList(),

                  sharedLifeRecord.Tags.Select(t => new TagSummaryDTO(t.TagId, t.TagName)).ToList(),

                  sharedLifeRecord.CreateTime, sharedLifeRecord.UpdateTime, sharedLifeRecord.DeleteTime
            );
        }
    }
}
