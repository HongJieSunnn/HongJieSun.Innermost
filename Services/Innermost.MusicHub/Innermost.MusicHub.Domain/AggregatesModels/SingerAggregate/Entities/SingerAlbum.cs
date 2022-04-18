namespace Innermost.MusicHub.Domain.AggregatesModels.SingerAggregate.Entities
{
    public class SingerAlbum : Entity<string>
    {
        [BsonId]
        [BsonRepresentation(MongoDB.Bson.BsonType.String)]
        public override string? Id { get => base.Id; set => base.Id = value; }
        public string AlbumName { get; private set; }
        public string AlbumDescriptions { get; private set; }
        public string AlbumGenre { get; private set; }
        public string AlbumLanguage { get; private set; }
        public int AlbumSongCount { get; private set; }
        public string PublishCompany { get; private set; }
        public string PublishTime { get; private set; }
        public SingerAlbum(string mid, string albumName, string albumDescriptions, string albumGenre, string albumLanguage, int albumSongCount, string publishCompany, string publishTime)
        {
            Id = mid;
            AlbumName = albumName;
            AlbumDescriptions = albumDescriptions;
            AlbumGenre = albumGenre;
            AlbumLanguage = albumLanguage;
            AlbumSongCount = albumSongCount > 0 ? albumSongCount : throw new ArgumentException("AlbumSongCount must be at least 1.");
            PublishCompany = publishCompany;
            PublishTime = publishTime;
        }
    }
}
