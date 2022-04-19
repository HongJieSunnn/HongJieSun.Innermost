using Innermost.IdempotentCommand.Infrastructure.Repositories;
using Innermost.Meet.Domain.AggregatesModels.SharedLifeRecordAggregate.ValueObjects;

namespace Innermost.Meet.API.Application.CommandHandlers
{
    public class LikeSharedLifeRecordCommandHandler : IRequestHandler<LikeSharedLifeRecordCommand, bool>
    {
        private readonly ISharedLifeRecordRepository _sharedLifeRecordRepository;
        private readonly IUserIdentityService _userIdentityService;
        public LikeSharedLifeRecordCommandHandler(ISharedLifeRecordRepository sharedLifeRecordRepository,IUserIdentityService userIdentityService)
        {
            _sharedLifeRecordRepository = sharedLifeRecordRepository;
            _userIdentityService = userIdentityService;

        }
        public async Task<bool> Handle(LikeSharedLifeRecordCommand request, CancellationToken cancellationToken)
        {
            var getUserNamesTask = _userIdentityService.GetUserNamesAsync();
            var getUserAvatarUrlTask=_userIdentityService.GetUserAvatarUrlAsync();

            var sharedRecord =await _sharedLifeRecordRepository.GetSharedLifeRecordAsync(request.SharedLifeRecordObjectId);

            var userNames = await getUserNamesTask;
            var userAvatarUrl = await getUserAvatarUrlTask;

            var like = new Like(request.LikerUserId!, userNames.userName, userNames.userNickName, userAvatarUrl, request.LikeTime);
            var update = sharedRecord.AddLike(like);

            var updateResult= await _sharedLifeRecordRepository.UpdateSharedLifeRecordAsync(request.SharedLifeRecordObjectId, update);

            var requestResult = updateResult.MatchedCount==1&&updateResult.ModifiedCount==2;
            if (!requestResult)
                throw new CommandHandleFailedException();//to roll back.

            await _sharedLifeRecordRepository.UnitOfWork.SaveEntitiesAsync(sharedRecord, cancellationToken);

            return requestResult;
        }
    }

    public class IdempotentLikeSharedLifeRecordCommandHandler : IdempotentCommandHandler<LikeSharedLifeRecordCommand, bool>
    {
        public IdempotentLikeSharedLifeRecordCommandHandler(IMediator mediator, ICommandRequestRepository commandRequestRepository) : base(mediator, commandRequestRepository)
        {
        }
    }
}
