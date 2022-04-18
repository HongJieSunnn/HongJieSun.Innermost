namespace Innermost.Meet.Domain.AggregatesModels.SharedLifeRecord.Entities
{
    public class MusicRecord:Entity<string>
    {
        [BsonId]
        [BsonRepresentation(MongoDB.Bson.BsonType.String)]
        public override string? Id { get => base.Id; set => base.Id = value; }
        public string MusicName { get; private set; }
        public string Singer { get; private set; }
        public string Album { get; private set; }

        public MusicRecord(string mid, string musicName, string singer, string album)
        {
            Id = mid;
            MusicName = musicName;
            Singer = singer;
            Album = album;
        }
    }
}
