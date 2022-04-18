using MongoDB.Driver.GeoJsonObjectModel;

namespace Innermost.Meet.Domain.AggregatesModels.SharedLifeRecord.Entities
{
    public class Location : Entity<string>
    {
        [BsonId]
        [BsonRepresentation(MongoDB.Bson.BsonType.String)]
        public override string? Id { get => base.Id; set => base.Id = value; }
        public string LocationName { get; private set; }
        public string Province { get; private set; }
        public string City { get; private set; }
        public string? District { get; private set; }
        public string Address { get; private set; }
        public GeoJsonPoint<GeoJson2DGeographicCoordinates> BaiduPOI { get; private set; }
        public Location(string uid, string locationName, string province, string city, string? district, string address, GeoJsonPoint<GeoJson2DGeographicCoordinates> baiduPOI)
        {
            Id = uid;
            LocationName = locationName;
            Province = province;
            City = city;
            District = district;
            Address = address;
            BaiduPOI = baiduPOI;
        }
    }
}
