namespace Innermost.LogLife.Domain.AggregatesModels.LifeRecordAggregate.ValueObjects
{
    public class BaiduPOI : ValueObject
    {
        public float Longitude { get; private set; }
        public float Latitude { get; private set; }
        public BaiduPOI(float longitude, float latitude)
        {
            Longitude = longitude;
            Latitude = latitude;
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Longitude;
            yield return Latitude;
        }
    }
}
