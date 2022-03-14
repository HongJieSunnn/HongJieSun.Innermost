using MongoDB.Bson.Serialization.Attributes;

namespace IntegrationEventRecordMongoDB
{
    public class IntegrationEventRecordMongoDBModel
    {
        [BsonId]
        [BsonIgnore]
        [BsonRepresentation(MongoDB.Bson.BsonType.ObjectId)]
        public string? Id { get; set; }

        [BsonRepresentation(MongoDB.Bson.BsonType.String)]
        public Guid EventId { get; set; }
        public string EventName { get; set; }
        public IntegrationEvent EventContent { get; set; }
        public EventState EventState { get; set; }
        public int TimeSend { get; set; }
        public DateTime CreateTime { get; set; }
        public IntegrationEventRecordMongoDBModel(Guid eventId,string eventName,IntegrationEvent eventContent,DateTime createTime)
        {
            EventId=eventId;
            EventName=eventName;
            EventContent=eventContent;
            CreateTime=createTime;
            EventState = EventState.NotPublished;
            TimeSend = 0;
        }
    }
}
