using Innermost.Meet.Domain.AggregatesModels.UserSocialContactAggregate.Enumerations;

namespace Innermost.Meet.Domain.AggregatesModels.UserSocialContactAggregate.Entities
{
    public class ConfidantRequest : Entity<string>
    {
        public string RequestUserId { get; init; }
        public string RequestMessage { get; init; }
        public ConfidantRequestStatue ConfidantRequestStatue { get; private set; }
        public DateTime RequestTime { get; init; }
        public DateTime? UpdateTime { get; private set; }
        public ConfidantRequest(string requestUserId, string requestMessage, ConfidantRequestStatue confidantRequestStatue, DateTime requestTime, DateTime? updateTime)
        {
            Id = ObjectId.GenerateNewId().ToString();
            RequestUserId = requestUserId;
            RequestMessage = requestMessage;
            ConfidantRequestStatue = confidantRequestStatue;
            RequestTime = requestTime;
            UpdateTime = updateTime;
        }

        public void SetConfidantRequestPassed()
        {
            ConfidantRequestStatue = ConfidantRequestStatue.Passed;
            UpdateTime = DateTime.Now;
        }

        public void SetConfidantRequestRefused()
        {
            ConfidantRequestStatue = ConfidantRequestStatue.Refused;
            UpdateTime = DateTime.Now;
        }

        public void SetConfidantRequestRefusedAndNotReceiveRequestAnyMore()
        {
            ConfidantRequestStatue = ConfidantRequestStatue.RefusedAndNotReceiveRequestAnyMore;
            UpdateTime = DateTime.Now;
        }
    }
}
