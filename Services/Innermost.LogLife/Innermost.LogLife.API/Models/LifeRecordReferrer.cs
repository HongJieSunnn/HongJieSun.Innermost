using MongoDB.Driver.GeoJsonObjectModel;

namespace Innermost.LogLife.API.Models
{
    public class LifeRecordReferrer : IReferrer
    {
        public string ReferrerName => "LifeRecord";
        public int Id { get; set; }
        public string UserId { get; set; }
        public string? Title { get; set; }
        public string Text { get; set; }
        public bool IsShared { get; set; }

        public string? LocationUId { get; set; }
        public string? LocationName { get; set; }
        public string? Province { get; set; }
        public string? City { get; set; }
        public string? District { get; set; }
        public string? Address { get; set; }
        public GeoJsonPoint<GeoJson2DGeographicCoordinates>? BaiduPOI { get; set; }

        public string? MusicRecordMId { get; set; }
        public string? MusicName { get; set; }
        public string? Singer { get; set; }
        public string? Album { get; set; }

        public List<string>? ImagePaths { get; set; }

        public DateTime CreateTime { get; set; }
        public DateTime? UpdateTime { get; set; }
        public DateTime? DeleteTime { get; set; }

        public LifeRecordReferrer(int recordId, string userId, string? title, string text,
            string? locationUId, string? locationName, string? province, string? city, string? district, string? address, float? longitude,float? latitude,
            string? musicId, string? musicName, string? singer, string? album,
            List<string>? imagePaths,
            DateTime createTime, DateTime? updateTime, DateTime? deleteTime)
        {
            Id = recordId; UserId = userId; Title = title; Text = text;

            LocationUId = locationUId; LocationName = locationName; Province = province; City = city; District = district; Address = address; 
            BaiduPOI = (longitude is null||latitude is null) ? null:GeoJson.Point(new GeoJson2DGeographicCoordinates(longitude.Value, latitude.Value));

            MusicRecordMId = musicId; MusicName = musicName; Singer = singer; Album = album;

            ImagePaths = imagePaths;

            CreateTime = createTime; UpdateTime = updateTime; DeleteTime = deleteTime;
        }
    }
}
