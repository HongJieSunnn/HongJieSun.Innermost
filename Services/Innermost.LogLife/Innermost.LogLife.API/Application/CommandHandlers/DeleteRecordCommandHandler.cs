namespace Innermost.LogLife.API.Application.CommandHandlers
{
    public class DeleteRecordCommandHandler : IRequestHandler<DeleteRecordCommand, bool>
    {
        private readonly ILifeRecordRepository _lifeRecordRepository;
        public DeleteRecordCommandHandler(ILifeRecordRepository lifeRecordRepository)
        {
            _lifeRecordRepository = lifeRecordRepository;

        }

        public async Task<bool> Handle(DeleteRecordCommand request, CancellationToken cancellationToken)
        {
            var record= await _lifeRecordRepository.DeleteAsync(request.RecordId,request.UserId);
            if (record is null)
                return false;
            return await _lifeRecordRepository.UnitOfWork.SaveEntitiesAsync(cancellationToken);
        }
    }

    public class IdempotentDeleteRecordCommandHandler : IdempotentCommandHandler<DeleteRecordCommand, bool>
    {
        public IdempotentDeleteRecordCommandHandler(IMediator mediator, ICommandRequestRepository commandRequestRepository) : base(mediator, commandRequestRepository)
        {
        }

        protected override bool CreateDefault()
        {
            return true;
        }
    }
}
