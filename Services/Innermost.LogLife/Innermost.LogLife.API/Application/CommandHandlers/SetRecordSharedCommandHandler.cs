namespace Innermost.LogLife.API.Application.CommandHandlers
{
    public class SetRecordSharedCommandHandler : IRequestHandler<SetRecordSharedCommand, bool>
    {
        private readonly ILifeRecordRepository _lifeRecordRepository;
        public SetRecordSharedCommandHandler(ILifeRecordRepository lifeRecordRepository)
        {
            _lifeRecordRepository = lifeRecordRepository;
        }
        public async Task<bool> Handle(SetRecordSharedCommand request, CancellationToken cancellationToken)
        {
            var record = await _lifeRecordRepository.GetRecordByIdAsync(request.RecordId);
            record.SetShared();
            return await _lifeRecordRepository.UnitOfWork.SaveEntitiesAsync(cancellationToken);
        }
    }

    public class IdempotentSetRecordSharedCommandHandler : IdempotentCommandHandler<SetRecordSharedCommand, bool>
    {
        public IdempotentSetRecordSharedCommandHandler(IMediator mediator, ICommandRequestRepository commandRequestRepository) : base(mediator, commandRequestRepository)
        {
        }
    }
}
