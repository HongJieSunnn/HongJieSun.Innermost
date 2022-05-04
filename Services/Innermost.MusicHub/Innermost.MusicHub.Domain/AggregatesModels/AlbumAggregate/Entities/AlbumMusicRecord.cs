namespace Innermost.MusicHub.Domain.AggregatesModels.AlbumAggregate.Entities
{
    public class AlbumMusicRecord : Entity<string>
    {
        public string MusicMid { get; private set; }
        public string MusicName { get; private set; }
        public string? TranslatedMusicName { get; private set; }
        public string Genre { get; private set; }
        public string Language { get; private set; }
        public string MusicUrl { get; private set; }

        [BsonRequired]
        [BsonElement("Singers")]
        private List<AlbumSinger> _singers;
        public IReadOnlyCollection<AlbumSinger> AlbumSingers => _singers;
        public AlbumMusicRecord(string mid, string musicName, string translatedMusicName, string genre, string language, string musicUrl, List<AlbumSinger> singers)
        {
            Id = ObjectId.GenerateNewId().ToString();
            MusicMid = mid;
            MusicName = musicName;
            TranslatedMusicName = translatedMusicName;
            Genre = genre;
            Language = language;
            MusicUrl = musicUrl;
            _singers = singers;
        }
    }
}
