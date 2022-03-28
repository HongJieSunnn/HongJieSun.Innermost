namespace Innemost.LogLife.API.Queries
{
    public interface ILifeRecordQueries
    {
        Task<LifeRecordDTO?> FindRecordByRecordId(int id);
        Task<IEnumerable<LifeRecordDTO>> GetAllRecordsAsync();
        Task<IEnumerable<LifeRecordDTO>> FindRecordsByTagIdAsync(string tagId);
        Task<IEnumerable<LifeRecordDTO>> FindRecordsByCreateTimeAsync(DateTimeToFind dateTime);
        Task<IEnumerable<LifeRecordDTO>> FindRecordsByKeywordAsync(string keyword);
    }
}
