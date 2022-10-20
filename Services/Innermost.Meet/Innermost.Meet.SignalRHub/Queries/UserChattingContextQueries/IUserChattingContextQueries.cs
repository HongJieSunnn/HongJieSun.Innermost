namespace Innermost.Meet.SignalRHub.Queries.UserChattingContextQueries
{
    public interface IUserChattingContextQueries
    {
        Task<IEnumerable<string>> GetAllChattingContextIdsOfUserAsync(string userId);
        Task<string> GetChattingContextIdOfUsers(string userId1, string userId2);
        Task<IEnumerable<ChattingRecordDTO>> GetChattingRecordsAsync(string chattingContextId, int page = 1, int limit = 50);
    }
}
