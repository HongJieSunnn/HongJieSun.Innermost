namespace Innermost.LogLife.API.Application.CommandHandlers
{
    public class CreateRecordCommandHandler : IRequestHandler<CreateRecordCommand, bool>
    {
        private readonly ILifeRecordRepository _lifeRecordRepository;
        public CreateRecordCommandHandler(ILifeRecordRepository lifeRecordRepository)
        {
            _lifeRecordRepository = lifeRecordRepository;

        }
        public Task<bool> Handle(CreateRecordCommand request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
