namespace Innermost.MusicHub.Crawler.Entities
{
    internal class SingerEntity
    {
        public int SingerId { get; set; }
        [BsonId]
        [BsonRepresentation(BsonType.String)]
        public string SingerMid { get; set; }
        public string SingerName { get; set; }
        public string SingerAlias { get; set; }
        public string SingerNationality { get; set; }
        public string SingerBirthplace { get; set; }
        public string SingerOccupation { get; set; }
        public string SingerBirthday { get; set; }
        public string SingerRepresentativeWorks { get; set; }
        public string SingerRegion { get; set; }
        public string SingerCoverUrl { get; set; }
        public SingerEntity()
        {

        }
        public SingerEntity(string mid,int singerId, string singerName, string singerAlias,
            string singerNationality, string singerBirthplace, string singerOccupation, string singerBirthday, string singerRepresentativeWorks,
            string singerRegion, string singerCoverUrl)
        {
            SingerMid = mid;
            SingerId = singerId;
            SingerName = singerName;
            SingerAlias = singerAlias;
            SingerNationality = singerNationality;
            SingerBirthplace = singerBirthplace;
            SingerOccupation = singerOccupation;
            SingerBirthday = singerBirthday;
            SingerRepresentativeWorks = singerRepresentativeWorks;
            SingerRegion = singerRegion;
            SingerCoverUrl = singerCoverUrl;
        }
    }
}
