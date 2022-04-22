using TagS.Microservices.Client.DomainSeedWork;
using TagS.Microservices.Client.Models;

namespace Innermost.Meet.Domain.AggregatesModels.UserChattingAggregate.Entities
{
    /// <summary>
    /// ChattingRecord tagable entity.
    /// Tag chatting record seems awesome.
    /// Id is object id.
    /// </summary>
    public class ChattingRecord : TagableEntity<string>
    {
        public string SendUserId { get; init; }
        public string RecordMessage { get; init; }
        public DateTime CreateTime { get; init; }
        public ChattingRecord(string sendUserId,string recordMessage,DateTime createTime,List<TagSummary> tagSummaries) : base(tagSummaries)
        {
            SendUserId = sendUserId;
            RecordMessage = recordMessage;
            CreateTime = createTime;
        }

        protected override IReferrer ToReferrer()
        {
            throw new NotImplementedException();
        }
    }
}
