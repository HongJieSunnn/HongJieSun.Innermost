using Innermost.Meet.Domain.AggregatesModels.SharedLifeRecordAggregate.ValueObjects;

namespace Innermost.Meet.API.Application.CommandHandlersAggregate
{
    public class LikeSharedLifeRecordCommandHandler : IRequestHandler<LikeSharedLifeRecordCommand, bool>
    {
        private readonly ISharedLifeRecordRepository _sharedLifeRecordRepository;
        private readonly IIdentityProfileService _userIdentityService;
        public LikeSharedLifeRecordCommandHandler(ISharedLifeRecordRepository sharedLifeRecordRepository, IIdentityProfileService userIdentityService)
        {
            _sharedLifeRecordRepository = sharedLifeRecordRepository;
            _userIdentityService = userIdentityService;

        }
        public async Task<bool> Handle(LikeSharedLifeRecordCommand request, CancellationToken cancellationToken)
        {
            var getUserNamesTask = _userIdentityService.GetUserNamesAsync();
            var getUserAvatarUrlTask = _userIdentityService.GetUserAvatarUrlAsync();

            var sharedRecord = await _sharedLifeRecordRepository.GetSharedLifeRecordAsync(request.SharedLifeRecordObjectId);
            if (sharedRecord is null)
                return false;

            if (sharedRecord.Likes.FirstOrDefault(l => l.LikerUserId == request.LikerUserId) is not null)
                return false;

            var userNames = await getUserNamesTask;
            var userAvatarUrl = await getUserAvatarUrlTask;

            var like = new Like(request.LikerUserId!, userNames.userName, userNames.userNickName, userAvatarUrl, request.LikeTime!.Value);
            var update = sharedRecord.AddLike(like);

            var updateResult = await _sharedLifeRecordRepository.UpdateSharedLifeRecordAsync(request.SharedLifeRecordObjectId, update);

            await _sharedLifeRecordRepository.UnitOfWork.SaveEntitiesAsync(sharedRecord, cancellationToken);

            return true;
        }
    }

    public class IdempotentLikeSharedLifeRecordCommandHandler : IdempotentCommandHandler<LikeSharedLifeRecordCommand, bool>
    {
        public IdempotentLikeSharedLifeRecordCommandHandler(IMediator mediator, ICommandRequestRepository commandRequestRepository) : base(mediator, commandRequestRepository)
        {
        }
    }
}
