using System.Linq.Expressions;

namespace Innermost.Meet.API.Queries.SharedLifeRecordQueries
{
    public interface IMeetSharedLifeRecordQueries
    {
        Task<IEnumerable<SharedLifeRecordDTO>> GetRandomSharedLifeRecordsAsync(int limit = 20, Expression<Func<SharedLifeRecord, bool>>? filter= null);
        Task<IEnumerable<SharedLifeRecordDTO>> GetSharedLifeRecordsByMusicRecordAsync(string musicRecordMid, int page = 1, int limit = 20, string sortBy = "Id");
        Task<IEnumerable<SharedLifeRecordDTO>> GetSharedLifeRecordsByLocationAsync(float longitude, float latitude, int page = 1, int limit = 20, double minDistance = 0, double maxDistance = 5000, string sortBy = "Id");
        Task<IEnumerable<SharedLifeRecordDTO>> GetSharedLifeRecordsByTagsAsync(IEnumerable<string> tagIds, int page = 1, int limit = 20, string sortBy = "Id");
    }
}
