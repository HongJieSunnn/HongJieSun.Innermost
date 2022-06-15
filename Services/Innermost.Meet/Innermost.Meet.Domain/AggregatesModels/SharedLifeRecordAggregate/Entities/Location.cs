using MongoDB.Driver.GeoJsonObjectModel;

namespace Innermost.Meet.Domain.AggregatesModels.SharedLifeRecordAggregate.Entities
{
    public class Location : Entity<string>
    {
        public string LocationUid { get; private set; }
        public string LocationName { get; private set; }
        public string Province { get; private set; }
        public string City { get; private set; }
        public string? District { get; private set; }
        public string Address { get; private set; }
        public GeoJsonPoint<GeoJson2DGeographicCoordinates> BaiduPOI { get; private set; }
        public Location(string uid, string locationName, string province, string city, string? district, string address, GeoJsonPoint<GeoJson2DGeographicCoordinates> baiduPOI)
        {
            Id = ObjectId.GenerateNewId().ToString();
            LocationUid = uid;
            LocationName = locationName;
            Province = province;
            City = city;
            District = district;
            Address = address;
            BaiduPOI = baiduPOI;
        }
    }
}
