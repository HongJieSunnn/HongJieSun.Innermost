namespace Innemost.LogLife.API.Queries
{
    public class LifeRecordQueries
        : ILifeRecordQueries
    {
        private readonly string _connectionString = string.Empty;
        private readonly IIdentityService _identityService;
        public LifeRecordQueries(string connectionString,IIdentityService identityService)
        {
            _connectionString = connectionString;
            _identityService = identityService;
        }

        public async Task<LifeRecordDTO?> FindRecordByRecordId(int id)
        {
            using var connection = new MySqlConnection(_connectionString);
            var userId = _identityService.GetUserId();//To avoid get the record that is not belonged to current user.
            var sql =
            @"SELECT 
                    lr.Id,lr.Title,lr.Text,lr.IsShared,lr.CreateTime,
                    lo.Id,lo.LocationName,lo.Province,lo.City,lo.District,lo.Address,lo.Location_Longitude,lo.Location_Latitude,
                    m.Id,m.MusicName,m.Singer,m.Album,
                    t.TagId,t.TagName
                    FROM #LifeRecords lr
                    INNER JOIN #Locations lo ON lr.LocationUId=lo.Id
                    INNER JOIN #MusicRecords m ON lr.MusicRecordMId=m.Id
                    INNER JOIN #ImagePaths i ON i.RecordId=lr.Id
                    INNER JOIN LifeRecordTagSummary lts ON lr.Id=lts.LifeRecordsId
                    INNER JOIN #Tags t ON t.TagId=lts.TagsId
                    WHERE l.Id=@id AND l.UserId=@userId";//TODO I do not know whether this sql statement is useful.
            var record = await connection.QueryAsync<LifeRecordDTO, LocationDTO, MusicRecordDTO, List<TagSummaryDTO>, LifeRecordDTO>(
                sql,
                (lr, lo, m, t) =>
                {
                    lr.Location = lo;
                    lr.MusicRecord = m;
                    lr.TagSummaries = t;
                    return lr;
                },
                param: new { id = id, userId = userId }
            );

            return record.FirstOrDefault();
        }

        public async Task<IEnumerable<LifeRecordDTO>> GetAllRecordsAsync()
        {
            using var connection = new MySqlConnection(_connectionString);
            var userId = _identityService.GetUserId();//To avoid get the record that is not belonged to current user.
            var sql =
            @"SELECT 
                    lr.Id,lr.Title,lr.Text,lr.IsShared,lr.CreateTime,
                    lo.Id,lo.LocationName,lo.Province,lo.City,lo.District,lo.Address,lo.Location_Longitude,lo.Location_Latitude,
                    m.Id,m.MusicName,m.Singer,m.Album,
                    t.TagId,t.TagName
                    FROM #LifeRecords lr
                    INNER JOIN #Locations lo ON lr.LocationUId=lo.Id
                    INNER JOIN #MusicRecords m ON lr.MusicRecordMId=m.Id
                    INNER JOIN #ImagePaths i ON i.RecordId=lr.Id
                    INNER JOIN LifeRecordTagSummary lts ON lr.Id=lts.LifeRecordsId
                    INNER JOIN #Tags t ON t.TagId=lts.TagsId
                    WHERE l.UserId=@userId";//TODO I do not know whether this sql statement is useful.
            var record = await connection.QueryAsync<LifeRecordDTO, LocationDTO, MusicRecordDTO, List<TagSummaryDTO>, LifeRecordDTO>(
                sql,
                (lr, lo, m, t) =>
                {
                    lr.Location = lo;
                    lr.MusicRecord = m;
                    lr.TagSummaries = t;
                    return lr;
                },
                param: new {userId = userId }
            );

            return record;
        }

        public async Task<IEnumerable<LifeRecordDTO>> FindRecordsByCreateTimeAsync(DateTimeToFind dateTime)
        {
            using var connection = new MySqlConnection(_connectionString);
            var userId = _identityService.GetUserId();
            var (startTime, endTime) = dateTime.GetStartAndEndTimePair();
            var sql = @"SELECT 
                    lr.Id,lr.Title,lr.Text,lr.IsShared,lr.CreateTime,
                    lo.Id,lo.LocationName,lo.Province,lo.City,lo.District,lo.Address,lo.Location_Longitude,lo.Location_Latitude,
                    m.Id,m.MusicName,m.Singer,m.Album,
                    t.TagId,t.TagName
                    FROM #LifeRecords lr
                    INNER JOIN #Locations lo ON lr.LocationUId=lo.Id
                    INNER JOIN #MusicRecords m ON lr.MusicRecordMId=m.Id
                    INNER JOIN #ImagePaths i ON i.RecordId=lr.Id
                    INNER JOIN LifeRecordTagSummary lts ON lr.Id=lts.LifeRecordsId
                    INNER JOIN #Tags t ON t.TagId=lts.TagsId
                    WHERE l.UserId=@userId AND l.CreateTime>=@startTime AND l.CreateTime<=@endTime";

            var records = await connection.QueryAsync<LifeRecordDTO, LocationDTO, MusicRecordDTO, List<TagSummaryDTO>, LifeRecordDTO>(
                 sql,
                 (lr, lo, m, t) =>
                 {
                     lr.Location = lo;
                     lr.MusicRecord = m;
                     lr.TagSummaries = t;
                     return lr;
                 },
                 param: new { userId = userId, startTime = startTime, endTime = endTime }
             );

            return records;
        }

        public async Task<IEnumerable<LifeRecordDTO>> FindRecordsByTagIdAsync(string tagId)
        {
            using var connection = new MySqlConnection(_connectionString);
            var userId = _identityService.GetUserId();
            var sql = @"SELECT 
                    lr.Id,lr.Title,lr.Text,lr.IsShared,lr.CreateTime,
                    lo.Id,lo.LocationName,lo.Province,lo.City,lo.District,lo.Address,lo.Location_Longitude,lo.Location_Latitude,
                    m.Id,m.MusicName,m.Singer,m.Album,
                    t.TagId,t.TagName
                    FROM #LifeRecords lr
                    INNER JOIN #Locations lo ON lr.LocationUId=lo.Id
                    INNER JOIN #MusicRecords m ON lr.MusicRecordMId=m.Id
                    INNER JOIN #ImagePaths i ON i.RecordId=lr.Id
                    INNER JOIN LifeRecordTagSummary lts ON lr.Id=lts.LifeRecordsId
                    INNER JOIN #Tags t ON t.TagId=lts.TagsId
                    WHERE l.UserId=@userId AND t.TagId=@tagId";

            var records = await connection.QueryAsync<LifeRecordDTO, LocationDTO, MusicRecordDTO, List<TagSummaryDTO>, LifeRecordDTO>(
                 sql,
                 (lr, lo, m, t) =>
                 {
                     lr.Location = lo;
                     lr.MusicRecord = m;
                     lr.TagSummaries = t;
                     return lr;
                 },
                 param: new { userId = userId, tagId = tagId }
             );

            return records;
        }

        public Task<IEnumerable<LifeRecordDTO>> FindRecordsByKeywordAsync(string keyword)
        {
            throw new NotImplementedException();//TODO
        }
    }
}
