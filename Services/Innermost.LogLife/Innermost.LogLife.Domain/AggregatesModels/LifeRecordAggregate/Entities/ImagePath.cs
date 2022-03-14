namespace Innermost.LogLife.Domain.AggregatesModels.LifeRecordAggregate.Entities
{
    public class ImagePath:Entity
    {
        public string Path { get;private set; }
        public int RecordId { get;private set; }
        public LifeRecord? LifeRecord { get; set; }
        public ImagePath(string path)
        {
            Path = path;
        }
    }
}
