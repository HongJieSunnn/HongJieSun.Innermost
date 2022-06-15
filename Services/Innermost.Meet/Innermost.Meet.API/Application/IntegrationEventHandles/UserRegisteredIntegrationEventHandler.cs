using Innermost.Meet.Domain.AggregatesModels.UserConfidantAggregate;
using Innermost.Meet.Domain.AggregatesModels.UserInteractionAggregate;
using MongoDB.Bson;

namespace Innermost.Meet.API.Application.IntegrationEventHandles
{
    public class UserRegisteredIntegrationEventHandler : IIntegrationEventHandler<UserRegisteredIntegrationEvent>
    {
        private readonly IUserInteractionRepository _userInteractionRepository;
        private readonly IUserSocialContactRepository _userSocialContactRepository;
        private const string AdminUserId = "13B8D30F-CFF8-20AB-8D40-1A64ADA8D067";
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

            if (@event.UserId == AdminUserId)//Admin need not add admin as confidant.
                return;

            //Add confidants with admin and registered user
            var adminUserSocialContact=await _userSocialContactRepository.GetUserSocialContactAsync(AdminUserId);
            var chattingContextId = ObjectId.GenerateNewId().ToString();
            var addConfidantTime = DateTime.Now;

            var addConfidantUpdate = userSocialContact.AddConfidant(new Confidant(AdminUserId, chattingContextId, addConfidantTime));
            var addConfidantUpdateAdmin=adminUserSocialContact.AddConfidant(new Confidant(@event.UserId, chattingContextId, addConfidantTime));

            await _userSocialContactRepository.UpdateUserSocialContactAsync(@event.UserId, addConfidantUpdate);
            await _userSocialContactRepository.UpdateUserSocialContactAsync(AdminUserId, addConfidantUpdateAdmin);

            await _userSocialContactRepository.UnitOfWork.SaveEntitiesAsync(userSocialContact);
        }
    }
}
