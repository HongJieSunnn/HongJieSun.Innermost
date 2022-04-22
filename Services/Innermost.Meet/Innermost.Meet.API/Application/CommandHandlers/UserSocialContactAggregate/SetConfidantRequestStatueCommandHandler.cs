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
            if(request.ConfidantRequestStatue == ConfidantRequestStatue.Passed)
            {
                var requestUser = await _userSocialContactRepository.GetUserSocialContactAsync(request.RequestUserId);

                var chattingContextId=ObjectId.GenerateNewId().ToString();

                var addConfidantTime=DateTime.Now;
                var addConfidantUpdate = userToSet.AddConfidant(new Confidant(request.RequestUserId, chattingContextId, addConfidantTime));
                requestUser.AddConfidant(new Confidant(request.UserId, chattingContextId, addConfidantTime));

                var usersToAddConfidant = new string[] { request.UserId!, request.RequestUserId };

                var addConfidantUpdateResult = await _userSocialContactRepository.UpdateManyUserSocialContactsAsync(usersToAddConfidant, addConfidantUpdate);

                await _userSocialContactRepository.UnitOfWork.SaveEntitiesAsync(userToSet, cancellationToken);//Pulish just one domain event to add chattingcontext for each other.
            }

            return string.Empty;
        }
    }
}
