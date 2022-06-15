namespace Innermost.MusicHub.API.Queries.AlbumQueries.Models
{
    public class AlbumSingerDTO
    {
        public string Mid { get; set; }
        public string SingerName { get; private set; }
        public AlbumSingerDTO(string mid, string singerName)
        {
            Mid = mid;
            SingerName = singerName;
        }
    }
}
