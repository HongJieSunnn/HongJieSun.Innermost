using Innemost.LogLife.API.Queries.Models;

namespace Innemost.LogLife.API.Queries
{
    public class LifeRecordQueries
        : ILifeRecordQueries
    {
        private readonly string _connectionString = string.Empty;
        private readonly IIdentityService _identityService;
        public LifeRecordQueries(string connectionString, IIdentityService identityService)
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
                    lo.Id as LocationUId,lo.LocationName,lo.Province,lo.City,lo.District,lo.Address,lo.BaiduPOI_Longitude as Longitude,lo.BaiduPOI_Latitude as Latitude,
                    m.Id as MusicRecordMId,m.MusicName,m.Singer,m.Album,
                    GROUP_CONCAT(i.Path) as ImagePaths,
                    GROUP_CONCAT(t.TagId,'-',t.TagName) as Tags
                    FROM LifeRecords lr
                    LEFT JOIN Locations lo ON lr.LocationUId=lo.Id
                    LEFT JOIN MusicRecords m ON lr.MusicRecordMId=m.Id
                    LEFT JOIN ImagePaths i ON i.RecordId=lr.Id
                    INNER JOIN LifeRecordTagSummary lts ON lr.Id=lts.EntitiesId
                    INNER JOIN TagSummaries t ON t.TagId=lts.TagsTagId
                    WHERE lr.Id=@id AND lr.UserId=@userId
                    GROUP BY lr.Id";
            var record = await connection.QueryAsync<dynamic>(
                sql,
                param: new { id = id, userId = userId }
            );

            return QueryModelMapper.MapToLifeRecordDTO(record.FirstOrDefault());
        }

        public async Task<IEnumerable<LifeRecordDTO>> GetAllRecordsAsync()
        {
            using var connection = new MySqlConnection(_connectionString);
            var userId = _identityService.GetUserId();
            var sql =
            @"SELECT      
                    lr.Id,lr.Title,lr.Text,lr.IsShared,lr.CreateTime,
                    lo.Id as LocationUId,lo.LocationName,lo.Province,lo.City,lo.District,lo.Address,lo.BaiduPOI_Longitude as Longitude,lo.BaiduPOI_Latitude as Latitude,
                    m.Id as MusicRecordMId,m.MusicName,m.Singer,m.Album,
                    GROUP_CONCAT(i.Path) as ImagePaths,
                    GROUP_CONCAT(t.TagId,'-',t.TagName) as Tags
                    FROM LifeRecords lr
                    LEFT JOIN Locations lo ON lr.LocationUId=lo.Id
                    LEFT JOIN MusicRecords m ON lr.MusicRecordMId=m.Id
                    LEFT JOIN ImagePaths i ON i.RecordId=lr.Id
                    INNER JOIN LifeRecordTagSummary lts ON lr.Id=lts.EntitiesId
                    INNER JOIN TagSummaries t ON t.TagId=lts.TagsTagId
                    WHERE lr.UserId=@userId
                    GROUP BY lr.Id";

            var records = await connection.QueryAsync<dynamic>(sql, new { userId = userId });

            return QueryModelMapper.MapToLifeRecordDTOs(records);
        }

        public async Task<IEnumerable<LifeRecordDTO>> FindRecordsByCreateTimeAsync(DateTimeToFind dateTime)
        {
            using var connection = new MySqlConnection(_connectionString);
            var userId = _identityService.GetUserId();
            var (startTime, endTime) = dateTime.GetStartAndEndTimePair();
            var sql = @"SELECT      
                    lr.Id,lr.Title,lr.Text,lr.IsShared,lr.CreateTime,
                    lo.Id as LocationUId,lo.LocationName,lo.Province,lo.City,lo.District,lo.Address,lo.BaiduPOI_Longitude as Longitude,lo.BaiduPOI_Latitude as Latitude,
                    m.Id as MusicRecordMId,m.MusicName,m.Singer,m.Album,
                    GROUP_CONCAT(i.Path) as ImagePaths,
                    GROUP_CONCAT(t.TagId,'-',t.TagName) as Tags
                    FROM LifeRecords lr
                    LEFT JOIN Locations lo ON lr.LocationUId=lo.Id
                    LEFT JOIN MusicRecords m ON lr.MusicRecordMId=m.Id
                    LEFT JOIN ImagePaths i ON i.RecordId=lr.Id
                    INNER JOIN LifeRecordTagSummary lts ON lr.Id=lts.EntitiesId
                    INNER JOIN TagSummaries t ON t.TagId=lts.TagsTagId
                    WHERE lr.UserId=@userId AND lr.CreateTime>=@startTime AND lr.CreateTime<=@endTime
                    GROUP BY lr.Id";

            var records = await connection.QueryAsync<dynamic>(
                 sql,
                 param: new { userId = userId, startTime = startTime, endTime = endTime }
             );

            return QueryModelMapper.MapToLifeRecordDTOs(records);
        }

        public async Task<IEnumerable<LifeRecordDTO>> FindRecordsByTagIdAsync(string tagId)
        {
            using var connection = new MySqlConnection(_connectionString);
            var userId = _identityService.GetUserId();
            var sql = @"SELECT      
                    lr.Id,lr.Title,lr.Text,lr.IsShared,lr.CreateTime,
                    lo.Id as LocationUId,lo.LocationName,lo.Province,lo.City,lo.District,lo.Address,lo.BaiduPOI_Longitude as Longitude,lo.BaiduPOI_Latitude as Latitude,
                    m.Id as MusicRecordMId,m.MusicName,m.Singer,m.Album,
                    GROUP_CONCAT(i.Path) as ImagePaths,
                    GROUP_CONCAT(t.TagId,'-',t.TagName) as Tags
                    FROM LifeRecords lr
                    LEFT JOIN Locations lo ON lr.LocationUId=lo.Id
                    LEFT JOIN MusicRecords m ON lr.MusicRecordMId=m.Id
                    LEFT JOIN ImagePaths i ON i.RecordId=lr.Id
                    INNER JOIN LifeRecordTagSummary lts ON lr.Id=lts.EntitiesId
                    INNER JOIN TagSummaries t ON t.TagId=lts.TagsTagId
                    WHERE lr.UserId=@userId AND t.TagId=@tagId
                    GROUP BY lr.Id";

            var records = await connection.QueryAsync<dynamic>(
                 sql,
                 param: new { userId = userId, tagId = tagId }
             );

            return QueryModelMapper.MapToLifeRecordDTOs(records);
        }

        public Task<IEnumerable<LifeRecordDTO>> FindRecordsByKeywordAsync(string keyword)
        {
            throw new NotImplementedException();//TODO
        }
    }
}
