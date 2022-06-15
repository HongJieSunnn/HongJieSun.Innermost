namespace Innermost.Meet.Domain.AggregatesModels.SharedLifeRecordAggregate.Entities
{
    public class MusicRecord : Entity<string>
    {
        public string MusicMid { get; private set; }
        public string MusicName { get; private set; }
        public string Singer { get; private set; }
        public string Album { get; private set; }

        public MusicRecord(string mid, string musicName, string singer, string album)
        {
            Id = ObjectId.GenerateNewId().ToString();
            MusicMid = mid;
            MusicName = musicName;
            Singer = singer;
            Album = album;
        }
    }
}
