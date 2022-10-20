namespace Innermost.Meet.API.Queries.SharedLifeRecordQueries.Models
{
    public class SharedLifeRecordDTO
    {
        public string SharedLifeRecordObjectId { get; init; }
        public int RecordId { get; init; }
        public string UserId { get; init; }
        public string UserName { get; init; }
        public string UserNickName { get; init; }
        public string UserAvatarUrl { get; init; }
        public string? Title { get; init; }
        public string Text { get; init; }
        public LocationDTO? Location { get; init; }
        public MusicRecordDTO? MusicRecord { get; init; }
        public List<string>? ImagePaths { get; init; }
        public int LikesCount { get; init; }
        public List<LikeDTO> Likes { get; init; }
        public List<TagSummaryDTO> TagSummaries { get; init; }
        public DateTime CreateTime { get; init; }
        public DateTime? UpdateTime { get; init; }
        public DateTime? DeleteTime { get; init; }

        public SharedLifeRecordDTO(
            string objectId, int recordId, string userId, string userName, string userNickName, string userAvatarUrl,
            string? title, string text,
            LocationDTO? location, MusicRecordDTO? musicRecord,
            List<string>? imagePaths, int likesCount, List<LikeDTO>? likes, List<TagSummaryDTO> tagSummaries,
            DateTime createTime, DateTime? updateTime, DateTime? deleteTime)
        {
            SharedLifeRecordObjectId = objectId;
            RecordId = recordId;
            UserId = userId;
            UserName = userName;
            UserNickName = userNickName;
            UserAvatarUrl = userAvatarUrl;
            Title = title;
            Text = text;
            Location = location;
            MusicRecord = musicRecord;
            ImagePaths = imagePaths;
            LikesCount = likesCount;
            Likes = likes ?? new List<LikeDTO>();
            TagSummaries = tagSummaries;
            CreateTime = createTime;
            UpdateTime = updateTime;
            DeleteTime = deleteTime;
        }

        public SharedLifeRecordDTO(SharedLifeRecord sharedLifeRecord)
        {

            SharedLifeRecordObjectId = sharedLifeRecord.Id!;
            RecordId = sharedLifeRecord.RecordId!;
            UserId = sharedLifeRecord.UserId!;
            UserName = sharedLifeRecord.UserName!;
            UserNickName = sharedLifeRecord.UserNickName!;
            UserAvatarUrl = sharedLifeRecord.UserAvatarUrl!;
            Title = sharedLifeRecord.Title;
            Text = sharedLifeRecord.Text;

            Location = (sharedLifeRecord.Location is null) ?
                null
                :
                new LocationDTO(sharedLifeRecord.Location.Id!,
                      sharedLifeRecord.Location.LocationName, sharedLifeRecord.Location.Province, sharedLifeRecord.Location.City, sharedLifeRecord.Location.District, sharedLifeRecord.Location.Address,
                      (float)sharedLifeRecord.Location.BaiduPOI.Coordinates.Longitude, (float)sharedLifeRecord.Location.BaiduPOI.Coordinates.Latitude);

            MusicRecord = (sharedLifeRecord.MusicRecord is null) ?
                null
                :
                new MusicRecordDTO(sharedLifeRecord.MusicRecord.Id!, sharedLifeRecord.MusicRecord.MusicName, sharedLifeRecord.MusicRecord.Singer, sharedLifeRecord.MusicRecord.Album);

            ImagePaths = sharedLifeRecord.ImagePaths?.ToList();
            LikesCount = sharedLifeRecord.LikesCount;
            Likes = sharedLifeRecord.Likes.Select(l => new LikeDTO(l.LikerUserId, l.LikerUserName, l.LikerUserNickName, l.LikerUserAvatarUrl, l.LikeTime)).ToList();
            TagSummaries = sharedLifeRecord.Tags.Select(t => new TagSummaryDTO(t.TagId, t.TagName)).ToList();
            CreateTime = sharedLifeRecord.CreateTime;
            UpdateTime = sharedLifeRecord.UpdateTime;
            DeleteTime = sharedLifeRecord.DeleteTime;
        }
    }

    public class LocationDTO
    {
        public string LocationUId { get; init; }
        public string LocationName { get; init; }
        public string Province { get; init; }
        public string City { get; init; }
        public string? District { get; init; }
        public string Address { get; init; }
        public float Longitude { get; init; }
        public float Latitude { get; init; }
        public LocationDTO(string uid, string locationName, string province, string city, string? district, string address, float longitude, float latitude)
        {
            LocationUId = uid;
            LocationName = locationName;
            Province = province;
            City = city;
            District = district;
            Address = address;
            Longitude = longitude;
            Latitude = latitude;
        }
    }

    public class MusicRecordDTO
    {
        public string MusicMId { get; init; }
        public string MusicName { get; init; }
        public string Singer { get; init; }
        public string Album { get; init; }

        public MusicRecordDTO(string mid, string musicName, string singer, string album)
        {
            MusicMId = mid;
            MusicName = musicName;
            Singer = singer;
            Album = album;
        }
    }

    public class LikeDTO
    {
        public string LikerUserId { get; init; }
        public string LikerUserName { get; init; }
        public string LikerUserNickName { get; init; }
        public string LikerUserAvatarUrl { get; init; }
        public DateTime LikeTime { get; init; }
        public LikeDTO(string likerUserId, string likerUserName, string likerUserNickeName, string likerUserAvatarUrl, DateTime likeTime)
        {
            LikerUserId = likerUserId;
            LikerUserName = likerUserName;
            LikerUserNickName = likerUserNickeName;
            LikerUserAvatarUrl = likerUserAvatarUrl;
            LikeTime = likeTime;
        }
    }

    public class TagSummaryDTO
    {
        public string TagId { get; init; }
        public string TagName { get; init; }
        public TagSummaryDTO(string tagId, string tagName)
        {
            TagId = tagId;
            TagName = tagName;
        }
    }

}
