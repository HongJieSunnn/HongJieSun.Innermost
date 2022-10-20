namespace Innermost.Meet.API.Application.Commands.UserSocialContactAggregate
{
    public class SetConfidantRequestStatueCommand : IRequest<string>
    {
        public string? UserId { get; set; }
        public string ConfidantRequestId { get; set; }
        public string RequestUserId { get; set; }
        public ConfidantRequestStatue ConfidantRequestStatue { get; set; }
        public SetConfidantRequestStatueCommand(string? userId, string confidantRequestId, string requestUserId, ConfidantRequestStatue confidantRequestStatue)
        {
            UserId = userId;
            ConfidantRequestId = confidantRequestId;
            RequestUserId = requestUserId;
            ConfidantRequestStatue = confidantRequestStatue;
        }
    }
}
