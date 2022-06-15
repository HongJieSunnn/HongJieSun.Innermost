namespace Innermost.MusicHub.API.Queries.MusicRecordQueries.Models
{
    public class MusicRecordSingerDTO
    {
        public string Mid { get; init; }
        public string SingerName { get; init; }
        public MusicRecordSingerDTO(string mid, string singerName)
        {
            Mid = mid;
            SingerName = singerName;
        }
    }
}
