namespace Innermost.MusicHub.API.Queries.SingerQueries.Models
{
    public class SingerDTO
    {
        public string Mid { get; set; }
        public long SingerId { get; private set; }
        public string SingerName { get; private set; }
        public string SingerAlias { get; private set; }
        public string SingerNationality { get; private set; }
        public string SingerBirthplace { get; private set; }
        public string SingerOccupation { get; private set; }
        public string SingerBirthday { get; private set; }
        public string SingerRepresentativeWorks { get; private set; }
        public string SingerRegion { get; private set; }
        public string SingerCoverUrl { get; private set; }
        public List<SingerAlbumDTO> SingerAlbums { get; init; }
        public SingerDTO(string mid, long singerId, string singerName, string singerAlias,
            string singerNationality, string singerBirthplace, string singerOccupation, string singerBirthday, string singerRepresentativeWorks,
            string singerRegion, string singerCoverUrl, List<SingerAlbumDTO> singerAlbums)
        {
            Mid = mid;
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
            SingerAlbums = singerAlbums;
        }
    }
}
