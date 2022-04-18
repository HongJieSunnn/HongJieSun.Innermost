namespace Innermost.Meet.API.Queries.SharedLifeRecordQueries
{
    public interface IOwnSharedLifeRecordQueries
    {
        Task<IEnumerable<SharedLifeRecordDTO>> GetAllOwnSharedLifeRecordsAsync();
        Task<IEnumerable<SharedLifeRecordDTO>> GetOwnSharedLifeRecordsAsync(int page=1,int limit=20);
        Task<IEnumerable<SharedLifeRecordDTO>> GetAllOwnSharedLifeRecordsByTimeAsync(DateTimeToFind dateTimeToFind);
    }
}
