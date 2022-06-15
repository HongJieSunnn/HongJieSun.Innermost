using Innermost.Meet.Domain.AggregatesModels.UserConfidantAggregate;
using MongoDB.Bson;

namespace Innermost.Meet.API.Application.CommandHandlers.UserSocialContactAggregate
{
    public class SetConfidantRequestStatueCommandHandler : IRequestHandler<SetConfidantRequestStatueCommand, string>
    {
        private readonly IUserSocialContactRepository _userSocialContactRepository;
        public SetConfidantRequestStatueCommandHandler(IUserSocialContactRepository userSocialContactRepository)
        {
            _userSocialContactRepository = userSocialContactRepository;

        }
        public async Task<string> Handle(SetConfidantRequestStatueCommand request, CancellationToken cancellationToken)
        {
            var userToSet = await _userSocialContactRepository.GetUserSocialContactAsync(request.UserId!);
            if (userToSet.Confidants.FirstOrDefault(c=>c.ConfidantUserId==request.RequestUserId) is not null)//to avoid request more than once.
                return string.Empty;

            FilterDefinition<UserSocialContact>? setStatueFilter = null;
            UpdateDefinition<UserSocialContact>? setStatueUpdate = null;

            if (request.ConfidantRequestStatue == ConfidantRequestStatue.ToBeReviewed)
                return "Can not set ConfidantRequest statue to ToBeReviewed.";

            if (request.ConfidantRequestStatue == ConfidantRequestStatue.Passed)
                (setStatueFilter, setStatueUpdate) = userToSet.SetConfidantRequestPassed(request.ConfidantRequestId);
            else if(request.ConfidantRequestStatue == ConfidantRequestStatue.Refused)
                (setStatueFilter, setStatueUpdate) = userToSet.SetConfidantRequestRefused(request.ConfidantRequestId);
            else if(request.ConfidantRequestStatue == ConfidantRequestStatue.RefusedAndNotReceiveRequestAnyMore)
                (setStatueFilter, setStatueUpdate) = userToSet.SetConfidantRequestRefusedAndNotReceiveRequestAnyMore(request.ConfidantRequestId);

            if (setStatueFilter is null || setStatueUpdate is null)
                return $"ConfidantRequest(Id:{request.ConfidantRequestId}) is not existed or the request's statue has been set.";

            var updateResult=await _userSocialContactRepository.UpdateUserSocialContactAsync(request.UserId!,setStatueUpdate,setStatueFilter);

            //if passed add confidant to each other.
            if(request.ConfidantRequestStatue == ConfidantRequestStatue.Passed&&!userToSet.Confidants.Any(c=>c.ConfidantUserId==request.RequestUserId))
            {
                var requestUser = await _userSocialContactRepository.GetUserSocialContactAsync(request.RequestUserId);

                var chattingContextId=ObjectId.GenerateNewId().ToString();

                var addConfidantTime=DateTime.Now;
                var addConfidantUpdateUserToSet = userToSet.AddConfidant(new Confidant(request.RequestUserId, chattingContextId, addConfidantTime));
                var addConfidantUpdateRequestUser=requestUser.AddConfidant(new Confidant(request.UserId!, chattingContextId, addConfidantTime));

                await _userSocialContactRepository.UpdateUserSocialContactAsync(request.UserId!, addConfidantUpdateUserToSet);
                await _userSocialContactRepository.UpdateUserSocialContactAsync(request.RequestUserId!, addConfidantUpdateRequestUser);

                await _userSocialContactRepository.UnitOfWork.SaveEntitiesAsync(userToSet, cancellationToken);//Pulish just one domain event to add chattingcontext for each other.
            }

            return string.Empty;
        }
    }

    public class IdempotentSetConfidantRequestStatueCommandHandler : IdempotentCommandHandler<SetConfidantRequestStatueCommand, string>
    {
        public IdempotentSetConfidantRequestStatueCommandHandler(IMediator mediator, ICommandRequestRepository commandRequestRepository) : base(mediator, commandRequestRepository)
        {
        }
    }
}
