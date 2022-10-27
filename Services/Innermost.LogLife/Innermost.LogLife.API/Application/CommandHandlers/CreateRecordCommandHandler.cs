using Innermost.LogLife.Domain.AggregatesModels.LifeRecordAggregate.ValueObjects;

namespace Innermost.LogLife.API.Application.CommandHandlers
{
    public class CreateRecordCommandHandler : IRequestHandler<CreateRecordCommand, bool>
    {
        private readonly ILifeRecordRepository _lifeRecordRepository;
        public CreateRecordCommandHandler(ILifeRecordRepository lifeRecordRepository)
        {
            _lifeRecordRepository = lifeRecordRepository;

        }
        public async Task<bool> Handle(CreateRecordCommand request, CancellationToken cancellationToken)
        {
            List<ImagePath>? imagePaths = request.ImagePaths?.Select(i => new ImagePath(i)).ToList();
            List<TagSummary<int, LifeRecord>> tagSummaries = request.TagSummaries!.Select(t => new TagSummary<int, LifeRecord>(t.Key, t.Value)).ToList();//at least one emotion tag.
            LifeRecord lifeRecord = new LifeRecord(request.UserId!, request.Title, request.Text, request.LocationUId, request.MusicId, request.CreateTime!.Value, null, null, request.IsShared, imagePaths, tagSummaries);

            if (request.LocationUId is not null)
            {
                Location location = new Location(
                    request.LocationUId,
                    request.LocationName ?? throw new NullReferenceException(nameof(request.LocationName)),
                    request.Province ?? throw new NullReferenceException(nameof(request.Province)),
                    request.City ?? throw new NullReferenceException(nameof(request.City)),
                    request.Address ?? throw new NullReferenceException(nameof(request.Address)),
                    new BaiduPOI(request.Longitude!.Value, request.Latitude!.Value),
                    request.District
                );

                lifeRecord.Location = location;
            }

            if (request.MusicId is not null)
            {
                MusicRecord musicRecord = new MusicRecord(
                    request.MusicId,
                    request.MusicName ?? throw new NullReferenceException(nameof(request.MusicName)),
                    request.Singer ?? throw new NullReferenceException(nameof(request.Singer)),
                    request.Album ?? throw new NullReferenceException(nameof(request.Album))
                );

                lifeRecord.MusicRecord = musicRecord;
            }

            await _lifeRecordRepository.AddAsync(lifeRecord);
            await _lifeRecordRepository.UnitOfWork.SaveChangesAsync();

            if (request.IsShared)
            {
                lifeRecord.SetShared();
            }

            foreach (var tag in lifeRecord.Tags)
            {
                lifeRecord.AddDomainEventForAddingTag(tag);
            }

            return await _lifeRecordRepository.UnitOfWork.SaveEntitiesAsync(cancellationToken, false);
        }
    }

    public class IdempotentCreateRecordCommandHandler : IdempotentCommandHandler<CreateRecordCommand, bool>
    {
        public IdempotentCreateRecordCommandHandler(IMediator mediator, ICommandRequestRepository commandRequestRepository) : base(mediator, commandRequestRepository)
        {
        }

        protected override bool CreateDefault()
        {
            return true;
        }
    }
}
