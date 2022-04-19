using Innermost.Meet.Domain.AggregatesModels.UserSocialContactAggregate.Enumerations;

namespace Innermost.Meet.Domain.AggregatesModels.UserSocialContactAggregate.Entities
{
    public class ConfidantRequest:Entity<string>
    {
        public string RequestUserId { get; init; }
        public string RequestMessage { get; init; }
        public ConfidantRequestStatue ConfidantRequestStatue { get; private set; }
        public DateTime RequestTime { get; init; }
        public ConfidantRequest(string requestUserId,string requestMessage,ConfidantRequestStatue confidantRequestStatue,DateTime? requestTime)
        {
            RequestUserId= requestUserId;
            RequestMessage= requestMessage;
            ConfidantRequestStatue= confidantRequestStatue;
            RequestTime = requestTime ?? DateTime.Now;
        }
    }
}
