namespace Innermost.MusicHub.Crawler.Entities
{
    internal class AlbumEntity
    {
        public int AlbumId { get; set; }
        [BsonId]
        [BsonRepresentation(BsonType.String)]
        public string AlbumMid { get; set; }
        public string AlbumName { get; set; }
        public string AlbumDescriptions { get; set; }
        public string AlbumGenre { get; set; }
        public string AlbumLanguage { get; set; }
        public int AlbumSongCount { get; set; }
        public string? AlbumCoverUrl { get; set; }
        public string PublishCompany { get; set; }
        public string PublishDate { get; set; }
        public string SingerName { get; set; }
        public string SingerMid { get; set; }
        public List<string> MusicRecordMids { get; set; }

        public AlbumEntity()
        {
            //If has not this constructor,there will throw exception:
            //6253f6634e2bcba30b3870b6 exited by exception: MessagePack.MessagePackSerializationException: Failed to serialize System.Object value.
            //--->System.TypeInitializationException: The type initializer for 'FormatterCache`1' threw an exception.
            //--->MessagePack.Internal.MessagePackDynamicObjectResolverException: can't find matched constructor. type:Innermost.MusicHub.Crawler.Entities.AlbumEntity
        }
        public AlbumEntity(
            int id, string mid, string albumName, string albumDescriptions,
            string albumGenre, string albumLanguage, string? albumCoverUrl,
            int albumSongCount, string publishCompany, string publishDate, string singerName, string singerMid, List<string> musicRecordMids)
        {
            AlbumId = id;
            AlbumMid = mid;
            AlbumName = albumName;
            AlbumDescriptions = albumDescriptions;
            AlbumGenre = albumGenre;
            AlbumLanguage = albumLanguage;
            AlbumCoverUrl = albumCoverUrl;
            AlbumSongCount = albumSongCount > 0 ? albumSongCount : throw new ArgumentException("AlbumSongCount must be at least 1.");
            PublishCompany = publishCompany;
            PublishDate = publishDate;
            SingerName = singerName;
            SingerMid = singerMid;
            MusicRecordMids = musicRecordMids;
        }
    }
}
