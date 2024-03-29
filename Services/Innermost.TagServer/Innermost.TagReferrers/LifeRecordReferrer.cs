﻿using MongoDB.Bson.Serialization.Attributes;

namespace Innermost.TagReferrers
{
    public class LifeRecordReferrer : ReferrerBase
    {
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
        [BsonIgnore]
        public float? Longitude { get; set; }
        [BsonIgnore]
        public float? Latitude { get; set; }

        [JsonIgnore]
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
            string? locationUId, string? locationName, string? province, string? city, string? district, string? address, float? longitude, float? latitude,
            string? musicId, string? musicName, string? singer, string? album,
            List<string>? imagePaths,
            DateTime createTime, DateTime? updateTime, DateTime? deleteTime):base(recordId)
        {
            UserId = userId; Title = title; Text = text;

            LocationUId = locationUId; LocationName = locationName; Province = province; City = city; District = district; Address = address;
            //Location can be null.However,the baiduPOI with geo index can not be null.
            //So if location is null,we add longitude and latitude of "天涯海角" to record.
            Longitude=longitude??(float)109.359673; Latitude=latitude?? (float)18.298693;

            MusicRecordMId = musicId; MusicName = musicName; Singer = singer; Album = album;

            ImagePaths = imagePaths;

            CreateTime = createTime; UpdateTime = updateTime; DeleteTime = deleteTime;
        }
    }

    /// <summary>
    /// Newtonsoft.Json can not deserialize GeoJsonPoint<GeoJson2DGeographicCoordinates> 
    /// because GeoJsonPoint<GeoJson2DGeographicCoordinates> has not a defualt construct(to deserialize instance by setting value to properties) and has not a constructor with attribute JsonConstructor
    /// So,we create the class inherit GeoJsonPoint<GeoJson2DGeographicCoordinates> and provide a constructor with attribute [JsonConstructor] and which is useful for deserializing.
    /// However,Mongodb can not use GeoJsonPointGeoJson2DGeographicCoordinates under Geo2DSphere index.
    /// </summary>
    public class GeoJsonPointGeoJson2DGeographicCoordinates : GeoJsonPoint<GeoJson2DGeographicCoordinates>
    {
        [JsonConstructor]
        public GeoJsonPointGeoJson2DGeographicCoordinates(GeoJson2DGeographicCoordinates coordinates) : base(coordinates)
        {
        }
    }
}
