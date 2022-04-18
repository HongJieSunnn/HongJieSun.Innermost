using Innermost.Meet.Domain.AggregatesModels.SharedLifeRecord;
using Innermost.Meet.Domain.AggregatesModels.SharedLifeRecord.Entities;
using MongoDB.Driver.GeoJsonObjectModel;
using TagS.Microservices.Client.Models;

namespace Innermost.Meet.API.Application.IntegrationEventHandles
{
    public class LifeRecordSetSharedIntegrationEventHandler : IIntegrationEventHandler<LifeRecordSetSharedIntegrationEvent>
    {
        private readonly ISharedLifeRecordRepository _sharedLifeRecordRepository;
        private readonly ILogger<LifeRecordSetSharedIntegrationEventHandler> _logger;
        public LifeRecordSetSharedIntegrationEventHandler(ISharedLifeRecordRepository sharedLifeRecordRepository, ILogger<LifeRecordSetSharedIntegrationEventHandler> logger)
        {
            _sharedLifeRecordRepository = sharedLifeRecordRepository;
            _logger = logger;

        }
        public async Task Handle(LifeRecordSetSharedIntegrationEvent @event)
        {
            _logger.LogIntegrationEventHandlerStartHandling(@event, Program.AppName);
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

            var sharedLifeRecord = new SharedLifeRecord(null, @event.RecordId, @event.UserId, @event.Title, @event.Text, location, musicRecord, @event.ImagePaths, null, tagSummaries, @event.CreateTime, null, null);

            await _sharedLifeRecordRepository.AddSharedLifeRecordAsync(sharedLifeRecord);
        }
    }
}
