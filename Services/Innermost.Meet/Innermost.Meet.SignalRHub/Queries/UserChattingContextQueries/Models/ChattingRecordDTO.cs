namespace Innermost.Meet.SignalRHub.Queries.UserChattingContextQueries.Models
{
    public class ChattingRecordDTO
    {
        public string ChattingRecordId { get; init; }
        public string SendUserId { get; init; }
        public string RecordMessage { get; init; }
        public List<TagSummaryDTO> TagSummaries { get; init; }
        public DateTime CreateTime { get; init; }
        public ChattingRecordDTO(string chattingRecordId,string sendUserId,string recordMessage,DateTime createTime, List<TagSummaryDTO>? tagSummaries)
        {
            ChattingRecordId=chattingRecordId;
            SendUserId=sendUserId;
            RecordMessage=recordMessage;
            TagSummaries=tagSummaries??new List<TagSummaryDTO>();
            CreateTime=createTime;
        }
    }

    public class TagSummaryDTO
    {
        public string TagId { get; init; }
        public string TagName { get; init; }
        public TagSummaryDTO(string tagId,string tagName)
        {
            TagId = tagId;
            TagName = tagName;
        }
    }
}
