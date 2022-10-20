using DomainSeedWork.Abstractions;

namespace Innermost.LogLife.Domain.AggregatesModels.LifeRecordAggregate.Entities
{
    public class MusicRecord
        : Entity<string>
    {
        public string MusicName { get; private set; }
        public string Singer { get; private set; }
        public string Album { get; private set; }
        public MusicRecord()
        {

        }

        public MusicRecord(string mid, string musicName, string singer, string album)
        {
            Id = mid;
            MusicName = musicName;
            Singer = singer;
            Album = album;
        }
    }
}
