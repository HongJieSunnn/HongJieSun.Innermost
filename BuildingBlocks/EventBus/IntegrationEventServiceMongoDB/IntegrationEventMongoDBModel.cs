using MongoDB.Bson.Serialization.Attributes;

namespace IntegrationEventServiceMongoDB
{
    public class IntegrationEventMongoDBModel
    {
        [BsonId]
        [BsonRepresentation(MongoDB.Bson.BsonType.String)]
        public Guid EventId { get; set; }
        public string EventName { get; set; }
        public string? SessionId { get; set; }
        public IntegrationEvent EventContent { get; set; }
        public EventState EventState { get; set; }
        public int TimeSend { get; set; }
        [BsonDateTimeOptions(Kind = DateTimeKind.Local)]
        public DateTime CreateTime { get; set; }
        public IntegrationEventMongoDBModel(Guid eventId, string eventName, IntegrationEvent eventContent, DateTime createTime, string? sessionId = null)
        {
            EventId = eventId;
            EventName = eventName;
            SessionId = sessionId;
            EventContent = eventContent;
            CreateTime = createTime;
            EventState = EventState.NotPublished;
            TimeSend = 0;
        }
    }
}
