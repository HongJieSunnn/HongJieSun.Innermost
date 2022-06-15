namespace Innermost.MusicHub.Domain.AggregatesModels.MusicRecordAggregate.Entities
{
    public class MusicRecordSinger : Entity<string>
    {
        public string SingerMid { get; private set; }
        public string SingerName { get; private set; }
        public MusicRecordSinger(string mid, string singerName)
        {
            Id = ObjectId.GenerateNewId().ToString();
            SingerMid = mid;
            SingerName = singerName;
        }
    }
}
