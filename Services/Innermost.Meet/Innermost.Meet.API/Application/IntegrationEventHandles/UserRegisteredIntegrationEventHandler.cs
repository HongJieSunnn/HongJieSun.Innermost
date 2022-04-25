using Innermost.Meet.Domain.AggregatesModels.UserConfidantAggregate;
using Innermost.Meet.Domain.AggregatesModels.UserInteraction;

namespace Innermost.Meet.API.Application.IntegrationEventHandles
{
    public class UserRegisteredIntegrationEventHandler : IIntegrationEventHandler<UserRegisteredIntegrationEvent>
    {
        private readonly IUserInteractionRepository _userInteractionRepository;
        private readonly IUserSocialContactRepository _userSocialContactRepository;
        public UserRegisteredIntegrationEventHandler(IUserInteractionRepository userInteractionRepository,IUserSocialContactRepository userSocialContactRepository)
        {
            _userInteractionRepository = userInteractionRepository;
            _userSocialContactRepository = userSocialContactRepository;

        }
        public async Task Handle(UserRegisteredIntegrationEvent @event)
        {
            var userInteraction = new UserInteraction(@event.UserId, null);
            var userSocialContact=new UserSocialContact(@event.UserId,null,null);

            await _userInteractionRepository.AddUserInteractionAsync(userInteraction);
            await _userSocialContactRepository.AddUserSocialContactAsync(userSocialContact);
        }
    }
}
