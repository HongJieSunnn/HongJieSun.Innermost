namespace Innermost.MusicHub.Domain.AggregatesModels.MusicRecordAggregate.Entities
{
    public class MusicRecordSinger:Entity<string>
    {
        [BsonId]
        [BsonRepresentation(MongoDB.Bson.BsonType.String)]
        public override string? Id { get => base.Id; set => base.Id = value; }
        public string SingerName { get; private set; }
        public string SingerRegion { get; private set; }
        public MusicRecordSinger(string mid,string singerName,string singerRegion)
        {
            Id=mid;
            SingerName=singerName;
            SingerRegion=singerRegion;
        }
    }
}
