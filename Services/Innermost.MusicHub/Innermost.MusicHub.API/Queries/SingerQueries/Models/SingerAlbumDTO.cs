namespace Innermost.MusicHub.API.Queries.SingerQueries.Models
{
    public class SingerAlbumDTO
    {
        public string Mid { get;private set; }
        public string AlbumName { get; private set; }
        public string AlbumDescriptions { get; private set; }
        public string AlbumGenre { get; private set; }
        public string AlbumLanguage { get; private set; }
        public string AlbumCoverUrl { get;private set; }
        public int AlbumSongCount { get; private set; }
        public string PublishCompany { get; private set; }
        public string PublishTime { get; private set; }
        public SingerAlbumDTO(string mid, string albumName, string albumDescriptions, string albumGenre, string albumLanguage,string albumCoverUrl, int albumSongCount, string publishCompany, string publishTime)
        {
            Mid = mid;
            AlbumName = albumName;
            AlbumDescriptions = albumDescriptions;
            AlbumGenre = albumGenre;
            AlbumLanguage = albumLanguage;
            AlbumCoverUrl = albumCoverUrl;
            AlbumSongCount = albumSongCount > 0 ? albumSongCount : throw new ArgumentException("AlbumSongCount must be at least 1.");
            PublishCompany = publishCompany;
            PublishTime = publishTime;
        }
    }
}
