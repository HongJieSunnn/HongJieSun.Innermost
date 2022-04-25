using Innermost.Meet.Domain.AggregatesModels.UserChattingAggregate;
using Innermost.Meet.Domain.Events.UserSocialContactEvents;

namespace Innermost.Meet.API.Application.DomainEventHandlers.ConfidantAdded
{
    public class AddConfidantDomainEventHandler : INotificationHandler<AddConfidantDomainEvent>
    {
        private readonly IUserChattingContextRepository _userChattingContextRepository;
        public AddConfidantDomainEventHandler(IUserChattingContextRepository userChattingContextRepository)
        {
            _userChattingContextRepository= userChattingContextRepository;
        }
        public async Task Handle(AddConfidantDomainEvent notification, CancellationToken cancellationToken)
        {
            var userChattingContext = new UserChattingContext(notification.ChattingContextId, notification.UserId1, notification.UserId2, null);

            await _userChattingContextRepository.AddUserChattingContextAsync(userChattingContext);
        }
    }
}
