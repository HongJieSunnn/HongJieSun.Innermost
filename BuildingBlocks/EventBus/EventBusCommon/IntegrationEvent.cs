using MongoDB.Bson.Serialization.Attributes;

namespace EventBusCommon
{
    public record IntegrationEvent
    {
        [JsonProperty]
        public Guid Id { get; private init; }

        [JsonProperty]
        [BsonDateTimeOptions(Kind = DateTimeKind.Local)]
        public DateTime CreationDate { get; private init; }

        public IntegrationEvent()
        {
            Id = Guid.NewGuid();
            CreationDate = DateTime.UtcNow;
        }

        [JsonConstructor]
        public IntegrationEvent(Guid id, DateTime creationDate)
        {
            Id = id;
            CreationDate = creationDate;
        }
    }
}
