namespace Innermost.Meet.API.Application.IntegrationEventHandles.IntegrationEvents
{
    public record LifeRecordSetSharedIntegrationEvent:IntegrationEvent
    {
        public int RecordId { get; private set; }
        public string UserId { get; private set; }
        public string? Title { get; private set; }
        public string Text { get; private set; }

        public string? LocationUId { get; private set; }
        public string? LocationName { get; private set; }
        public string? Province { get; private set; }
        public string? City { get; private set; }
        public string? District { get; private set; }
        public string? Address { get; private set; }
        public float? Longitude { get; private set; }
        public float? Latitude { get; private set; }

        public string? MusicId { get; private set; }
        public string? MusicName { get; private set; }
        public string? Singer { get; private set; }
        public string? Album { get; private set; }

        public List<string>? ImagePaths { get; private set; }

        public DateTime CreateTime { get; private set; }
        public DateTime? UpdateTime { get; private set; }
        public DateTime? DeleteTime { get; private set; }

        public List<(string TagId, string TagName)> TagSummaries { get; private set; }

        public LifeRecordSetSharedIntegrationEvent(
            int recordId, string userId, string? title, string text,
            string? locationUId, string? locationName, string? province, string? city, string? district, string? address, float? longitude, float? latitude,
            string? musicId, string? musicName, string? singer, string? album,
            List<string>? imagePaths,
            DateTime createTime, DateTime? updateTime, DateTime? deleteTime,
            List<(string TagId, string TagName)> tagSummaries)
        {
            RecordId = recordId; UserId = userId; Title = title; Text = text;

            LocationUId = locationUId; LocationName = locationName; Province = province; City = city; District = district; Address = address; Longitude = longitude; Latitude = latitude;

            MusicId = musicId; MusicName = musicName; Singer = singer; Album = album;

            ImagePaths = imagePaths;

            CreateTime = createTime; UpdateTime = updateTime; DeleteTime = deleteTime;

            TagSummaries = tagSummaries;
        }
    }
}
