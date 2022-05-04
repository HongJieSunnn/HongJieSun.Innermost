namespace Innermost.MusicHub.Domain.AggregatesModels.AlbumAggregate.Entities
{
    public class AlbumSinger : Entity<string>
    {
        public string SingerMid { get; private set; }
        public string SingerName { get; private set; }
        public AlbumSinger(string mid, string singerName)
        {
            Id = ObjectId.GenerateNewId().ToString();
            SingerMid = mid;
            SingerName = singerName;
        }
    }
}
