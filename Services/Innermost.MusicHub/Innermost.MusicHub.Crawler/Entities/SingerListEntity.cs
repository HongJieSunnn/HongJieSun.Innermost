namespace Innermost.MusicHub.Crawler.Entities
{
    internal class SingerListEntity
    {
        [BsonId]
        [BsonRepresentation(BsonType.String)]
        public string SingerMid { get; set; }
        public string Region { get; set; }
        public SingerListEntity()
        {

        }
        public SingerListEntity(string singerMid, string region)
        {
            SingerMid = singerMid;
            Region = region;
        }
    }
}
