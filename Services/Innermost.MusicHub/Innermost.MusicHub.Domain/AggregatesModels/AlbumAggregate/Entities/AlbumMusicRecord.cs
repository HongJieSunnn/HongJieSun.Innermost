

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
        public string Lyricist { get; private set; }
        public string Composer { get; private set; }
        public string Arranger { get; private set; }
        public string Genre { get; private set; }
        public string Language { get; private set; }
        public string MusicUrl { get; private set; }

        private readonly List<AlbumSinger> _singers;
        public IReadOnlyCollection<AlbumSinger> Singers => _singers;
        public AlbumMusicRecord(string mid, string musicName, string translatedMusicName, string lyricist, string composer, string arranger, string genre, string language, string musicUrl, List<AlbumSinger> singers)
        {
            Id = mid;
            MusicName = musicName;
            TranslatedMusicName = translatedMusicName;
            Lyricist = lyricist;
            Composer = composer;
            Arranger = arranger;
            Genre = genre;
            Language = language;
            MusicUrl = musicUrl;
            _singers = (singers is not null || singers?.Count > 0) ? singers : throw new ArgumentException("Singers must have at least one singer and can not be null");
        }
    }
}
