namespace Innermost.Meet.API.Application.CommandHandlers.UserSocialContactAggregate
{
    public class AddConfidantRequestCommandHandler : IRequestHandler<AddConfidantRequestCommand, string>
    {
        private readonly IUserSocialContactRepository _userSocialContactRepository;
        public AddConfidantRequestCommandHandler(IUserSocialContactRepository userSocialContactRepository)
        {
            _userSocialContactRepository = userSocialContactRepository;
        }

        public async Task<string> Handle(AddConfidantRequestCommand request, CancellationToken cancellationToken)
        {
            var userToAdd =await _userSocialContactRepository.GetUserSocialContactAsync(request.ToUserId);
            if (userToAdd is null)
                return $"User(Id:{request.ToUserId}) is not existed.";

            if (userToAdd.Confidants.FirstOrDefault(c => c.Id == request.RequestUserId) is not null)
                return $"User(Id:{request.ToUserId}) has already been confidant of User(Id:{request.RequestUserId}).";

            if(userToAdd.ConfidantRequests.FirstOrDefault(cr => cr.RequestUserId == request.RequestUserId && cr.ConfidantRequestStatue == ConfidantRequestStatue.ToBeReviewed) is not null)
                return $"Confidant request has already existed between RequestUser(Id:{request.RequestUserId}) and ToUser(Id:{request.ToUserId}).";

            if(userToAdd.ConfidantRequests.FirstOrDefault(cr=>cr.RequestUserId==request.RequestUserId&&cr.ConfidantRequestStatue==ConfidantRequestStatue.RefusedAndNotReceiveRequestAnyMore) is not null)
                return $"User(Id:{request.ToUserId}) doesn't receive confidant requests from User(Id:{request.RequestUserId})";

            var confidantRequest = new ConfidantRequest(request.RequestUserId!, request.RequestMessage, ConfidantRequestStatue.ToBeReviewed, request.RequestTime!.Value,null);
            var update= userToAdd.AddConfidantRequest(confidantRequest);

            var updateResult = await _userSocialContactRepository.UpdateUserSocialContactAsync(request.ToUserId, update);

            if (updateResult.MatchedCount != 1 || updateResult.ModifiedCount != 1)
                throw new CommandHandleFailedException();

            return string.Empty;
        }
    }
}
