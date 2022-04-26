

namespace Innermost.MusicHub.Domain.AggregatesModels.AlbumAggregate.Entities
{
    public class AlbumMusicRecord : Entity<string>
    {
        [BsonId]
        [BsonRepresentation(MongoDB.Bson.BsonType.String)]
        public override string? Id
        {
            get => base.Id;
            set => base.Id = value;
        }
        public string MusicName { get; private set; }
        public string? TranslatedMusicName { get; private set; }
        public string Genre { get; private set; }
        public string Language { get; private set; }
        public string MusicUrl { get; private set; }

        [BsonRequired]
        [BsonElement("Singers")]
        private readonly List<AlbumSinger> _singers;
        public IReadOnlyCollection<AlbumSinger> AlbumSingers => _singers;
        public AlbumMusicRecord(string mid, string musicName, string translatedMusicName, string genre, string language, string musicUrl, List<AlbumSinger> singers)
        {
            Id = mid;
            MusicName = musicName;
            TranslatedMusicName = translatedMusicName;
            Genre = genre;
            Language = language;
            MusicUrl = musicUrl;
            _singers = singers;
        }
    }
}
