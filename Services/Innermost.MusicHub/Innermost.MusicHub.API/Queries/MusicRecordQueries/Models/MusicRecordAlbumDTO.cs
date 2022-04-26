namespace Innermost.MusicHub.API.Queries.MusicRecordQueries.Models
{
    public class MusicRecordAlbumDTO
    {
        public string Mid { get; init; }
        public string AlbumName { get; init; }
        public string AlbumDescriptions { get; init; }
        public string AlbumGenre { get; init; }
        public string AlbumLanguage { get; init; }
        public string AlbumSingerName { get; init; }
        public string AlbumSingerMid { get; init; }
        public int AlbumSongCount { get; init; }
        public string PublishCompany { get; init; }
        public string PublishTime { get; init; }
        public MusicRecordAlbumDTO(string mid, string albumName, string albumDescriptions, string albumGenre, string albumLanguage, string albumSingerName, string albumSingerMid, int albumSongCount, string publishCompany, string publishTime)
        {
            Mid = mid;
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
