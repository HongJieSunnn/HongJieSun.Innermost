using MongoDB.Bson.Serialization.Attributes;
using System.Text.Json.Serialization;

namespace EventBusCommon
{
    public record IntegrationEvent
    {
        [JsonInclude]
        public Guid Id { get; private init; }

        [JsonInclude]
        [BsonDateTimeOptions(Kind = DateTimeKind.Local)]
        public DateTime CreationDate { get; private init; }

        public IntegrationEvent()
        {
            Id = Guid.NewGuid();
            CreationDate = DateTime.Now;
        }

        [JsonConstructor]
        public IntegrationEvent(Guid id, DateTime creationDate)
        {
            Id = id;
            CreationDate = creationDate;
        }
    }
}
