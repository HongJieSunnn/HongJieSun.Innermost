namespace Innermost.Meet.Domain.Events.UserSocialContactEvents
{
    public class AddConfidantDomainEvent:INotification
    {
        public string UserId1 { get; init; }
        public string UserId2 { get; init; }
        public string ChattingContextId { get; init; }
        public AddConfidantDomainEvent(string userId1,string userId2,string chattingContextId)
        {
            UserId1 = userId1;
            UserId2 = userId2;
            ChattingContextId = chattingContextId;
        }
    }
}
