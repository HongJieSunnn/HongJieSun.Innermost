namespace Innermost.Meet.SignalRHub.Application.Commonds.UserChattingContextAggregate
{
    public class PersistReceivedChattingRecordToMongoDBCommand:IRequest<bool>
    {
        public string ChattingContextId { get; init; }
        public List<ChattingRecordDTO> ChattingRecordDTOs { get; init; }
        public PersistReceivedChattingRecordToMongoDBCommand(string chattingContextId,List<ChattingRecordDTO> chattingRecordDTOs)
        {
            ChattingContextId = chattingContextId;
            ChattingRecordDTOs = chattingRecordDTOs;
        }
    }
}
