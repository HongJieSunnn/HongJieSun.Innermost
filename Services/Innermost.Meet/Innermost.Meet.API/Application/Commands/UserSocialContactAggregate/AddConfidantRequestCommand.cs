namespace Innermost.Meet.API.Application.Commands.UserSocialContactAggregate
{
    public class AddConfidantRequestCommand : IRequest<string>
    {
        public string? RequestUserId { get; set; }
        public string ToUserId { get; set; }
        public string RequestMessage { get; set; }
        public DateTime? RequestTime { get; set; }
        public AddConfidantRequestCommand(string? requestUserId, string toUserId, string requestMessage, DateTime? requestTime)
        {
            RequestUserId = requestUserId;
            ToUserId = toUserId;
            RequestMessage = requestMessage;
            RequestTime = requestTime ?? DateTime.Now;
        }
    }
}
