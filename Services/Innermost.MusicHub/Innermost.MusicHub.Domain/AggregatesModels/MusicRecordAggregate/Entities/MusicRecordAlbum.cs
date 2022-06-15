namespace Innermost.MusicHub.Domain.AggregatesModels.MusicRecordAggregate.Entities
{
    public class MusicRecordAlbum : Entity<string>
    {
        public string AlbumMid { get; private set; }
        public string AlbumName { get; private set; }
        public string AlbumDescriptions { get; private set; }
        public string AlbumGenre { get; private set; }
        public string AlbumLanguage { get; private set; }
        public string AlbumSingerName { get; private set; }
        public string AlbumSingerMid { get; private set; }
        public int AlbumSongCount { get; private set; }
        public string PublishCompany { get; private set; }
        public string PublishTime { get; private set; }
        public MusicRecordAlbum(string mid, string albumName, string albumDescriptions, string albumGenre, string albumLanguage,string albumSingerName,string albumSingerMid, int albumSongCount, string publishCompany, string publishTime)
        {
            Id = ObjectId.GenerateNewId().ToString();
            AlbumMid = mid;
            AlbumName = albumName;
            AlbumDescriptions = albumDescriptions;
            AlbumGenre = albumGenre;
            AlbumLanguage = albumLanguage;
            AlbumSingerName = albumSingerName;
            AlbumSingerMid = albumSingerMid;
            AlbumSongCount = albumSongCount > 0 ? albumSongCount : throw new ArgumentException("AlbumSongCount must be at least 1.");
            PublishCompany = publishCompany;
            PublishTime = publishTime;
        }
    }
}
