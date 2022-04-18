namespace Innermost.MusicHub.Domain.AggregatesModels.AlbumAggregate.Entities
{
    public class AlbumSinger:Entity<string>
    {
        [BsonId]
        [BsonRepresentation(MongoDB.Bson.BsonType.String)]
        public override string? Id { get => base.Id; set => base.Id = value; }
        public string SingerName { get; private set; }
        public string SingerRegion { get; private set; }
        public AlbumSinger(string mid, string singerName, string singerRegion)
        {
            Id = mid;
            SingerName = singerName;
            SingerRegion = singerRegion;
        }
    }
}
