namespace Innermost.MusicHub.Crawler.Entities
{
    internal class MusicRecordEntity
    {
        public int MusicId { get; set; }
        [BsonId]
        [BsonRepresentation(BsonType.String)]
        public string MusicMid { get; set; }
        public string MusicName { get; set; }
        public string? TranslatedMusicName { get; set; }
        public string? Introduction { get; set; }
        public string? Genre { get; set; }
        public string? Language { get; set; }
        public string AlbumCoverUrl { get; set; }
        public string MusicUrl { get; set; }
        public string? WikiUrl { get; set; }
        public string? Lyric { get; set; }
        public string AlbumMid { get; set; }
        public List<string> SingerMids { get; set; }
        public List<string> SingerNames { get; set; }

        public string? PublishTime { get; set; }
        public MusicRecordEntity()
        {

        }
        public MusicRecordEntity(
            string mid, int musicId,
            string musicName, string? translatedMusicName, string? introduction,
            string? genre, string? language, string albumMid,
            string albumCoverUrl, string musicUrl, string? wikiUrl, string? lyric, List<string> singerMids, List<string> singerNames,
            string? publishTime)
        {
            MusicMid = mid;
            MusicId = musicId;
            MusicName = musicName;
            TranslatedMusicName = translatedMusicName;
            Introduction = introduction;
            Genre = genre;
            Language = language;
            AlbumMid = albumMid;
            AlbumCoverUrl = albumCoverUrl;
            MusicUrl = musicUrl;
            Lyric = lyric;
            SingerMids = singerMids;
            SingerNames = singerNames;
            PublishTime = publishTime;
            WikiUrl = wikiUrl;
        }
    }
}
