namespace Innermost.MusicHub.API.Queries.AlbumQueries.Models
{
    public class AlbumDTO
    {
        public string Mid { get; set; }
        public long AlbumId { get; private set; }
        public string AlbumName { get; private set; }
        public string AlbumDescriptions { get; private set; }
        public string AlbumGenre { get; private set; }
        public string AlbumLanguage { get; private set; }
        public int AlbumSongCount { get; private set; }
        public string AlbumCoverUrl { get; private set; }
        public string AlbumSingerName { get; set; }
        public string AlbumSingerMid { get; set; }
        public string PublishCompany { get; private set; }
        public string PublishTime { get; private set; }
        public List<AlbumMusicRecordDTO> AlbumMusicRecords { get; set; }
        public AlbumDTO(
            string mid, long albumId, string albumName, string albumDescriptions,
            string albumGenre, string albumLanguage, string albumCoverUrl,
            string albumSingerName, string albumSingerMid,
            int albumSongCount, string publishCompany, string publishTime,
            List<AlbumMusicRecordDTO> musicRecords)
        {
            Mid = mid;
            AlbumId = albumId;
            AlbumName = albumName;
            AlbumDescriptions = albumDescriptions;
            AlbumGenre = albumGenre;
            AlbumLanguage = albumLanguage;
            AlbumCoverUrl = albumCoverUrl;
            AlbumSongCount = albumSongCount > 0 ? albumSongCount : throw new ArgumentException("AlbumSongCount must be at least 1.");
            AlbumSingerName = albumSingerName;
            AlbumSingerMid = albumSingerMid;
            PublishCompany = publishCompany;
            PublishTime = publishTime;
            AlbumMusicRecords = musicRecords;
        }
    }
}
