namespace Innermost.Meet.SignalRHub.Infrastructure.Services
{
    public interface IChattingRecordRedisService
    {
        Task<long> AddChattingRecordAsync(string chattingContextId, string chattingRecord);
        Task<long> AddChattingRecordAsync(string chattingContextId, ChattingRecordDTO chattingRecordDTO, bool received);
        Task<long> AddChattingRecordAsync(string chattingContextId, string sendUserId, string message, bool received);


        Task<IEnumerable<ChattingRecordDTO>> GetAllChattingRecordsAsync(string chattingContextId);

        Task<IEnumerable<ChattingRecordDTO>> GetNotReceivedChattingRecordsAsync(string chattingContextId, string connectedUserId);

        Task<IEnumerable<ChattingRecordDTO>> GetNotReceivedChattingRecordsAndSetAsReceivedAsync(string chattingContextId, string connectedUserId);

        Task SetNotReceivedChattingRecordsReceivedAsync(string chattingContextId, int modifyCount);

        Task PersistReceivedChattingRecordToMongoDBAsync(string chattingContextId);
    }
}
