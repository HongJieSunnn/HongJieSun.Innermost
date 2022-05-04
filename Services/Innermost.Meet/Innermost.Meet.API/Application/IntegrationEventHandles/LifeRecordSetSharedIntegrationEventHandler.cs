using Innermost.Meet.Domain.AggregatesModels.SharedLifeRecordAggregate.Entities;
using MongoDB.Driver.GeoJsonObjectModel;
using TagS.Microservices.Client.Models;

namespace Innermost.Meet.API.Application.IntegrationEventHandles
{
    public class LifeRecordSetSharedIntegrationEventHandler : IIntegrationEventHandler<LifeRecordSetSharedIntegrationEvent>
    {
        private readonly ISharedLifeRecordRepository _sharedLifeRecordRepository;
        private readonly IUserIdentityService _userIdentityService;
        private readonly ILogger<LifeRecordSetSharedIntegrationEventHandler> _logger;
        public LifeRecordSetSharedIntegrationEventHandler(ISharedLifeRecordRepository sharedLifeRecordRepository, IUserIdentityService userIdentityService, ILogger<LifeRecordSetSharedIntegrationEventHandler> logger)
        {
            _sharedLifeRecordRepository = sharedLifeRecordRepository;
            _userIdentityService = userIdentityService;
            _logger = logger;

        }
        public async Task Handle(LifeRecordSetSharedIntegrationEvent @event)
        {
            _logger.LogIntegrationEventHandlerStartHandling(@event, Program.AppName);

            var getUserNamesTask = _userIdentityService.GetUserNamesAsync(@event.UserId);
            var getUserAvatarUrlTask = _userIdentityService.GetUserAvatarUrlAsync(@event.UserId);

            Location? location = null;
            MusicRecord? musicRecord = null;
            List<TagSummary> tagSummaries = @event.TagSummaries.Select(t => new TagSummary(t.TagId, t.TagName)).ToList();

            if (@event.LocationUId is not null)
            {
                var baiduPOI = new GeoJsonPoint<GeoJson2DGeographicCoordinates>(new GeoJson2DGeographicCoordinates(@event.Longitude!.Value, @event.Latitude!.Value));
                location = new Location(@event.LocationUId, @event.LocationName!, @event.Province!, @event.City!, @event.District, @event.Address!, baiduPOI);
            }

            if (@event.MusicId is not null)
            {
                musicRecord = new MusicRecord(@event.MusicId, @event.MusicName!, @event.Singer!, @event.Album!);
            }

            var userNames = await getUserNamesTask;
            var userAvatarUrl = await getUserAvatarUrlTask;

            var sharedLifeRecord = new SharedLifeRecord(null, @event.RecordId, @event.UserId, userNames.userName, userNames.userNickName, userAvatarUrl, @event.Title, @event.Text, location, musicRecord, @event.ImagePaths, 0, null, tagSummaries, @event.CreateTime, null, null);

            await _sharedLifeRecordRepository.AddSharedLifeRecordAsync(sharedLifeRecord);
        }
    }
}
