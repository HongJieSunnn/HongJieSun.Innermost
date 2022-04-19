namespace Innermost.Meet.API.Queries.Models
{
    public class SharedLifeRecordDTO
    {
        public string SharedLifeRecordObjectId { get; set; }
        public int RecordId { get; set; }
        public string UserId { get; set; }
        public string UserName { get; set; }
        public string UserNickName { get; set; }
        public string UserAvatarUrl { get; set; }
        public string? Title { get; set; }
        public string Text { get; set; }
        public LocationDTO? Location { get; set; }
        public MusicRecordDTO? MusicRecord { get; set; }
        public List<string>? ImagePaths { get; set; }
        public int LikesCount { get; set; }
        public List<LikeDTO> Likes { get; set; }
        public List<TagSummaryDTO> TagSummaries { get; set; }
        public DateTime CreateTime { get; set; }
        public DateTime? UpdateTime { get; set; }
        public DateTime? DeleteTime { get; set; }

        public SharedLifeRecordDTO(
            string objectId, int recordId, string userId,string userName,string userNickName,string userAvatarUrl,
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
            Likes = sharedLifeRecord.Likes.Select(l => new LikeDTO(l.LikerUserId, l.LikerUserName,l.LikerUserNickName, l.LikerUserAvatarUrl, l.LikeTime)).ToList();
            TagSummaries = sharedLifeRecord.Tags.Select(t => new TagSummaryDTO(t.TagId, t.TagName)).ToList();
            CreateTime = sharedLifeRecord.CreateTime;
            UpdateTime = sharedLifeRecord.UpdateTime;
            DeleteTime = sharedLifeRecord.DeleteTime;
        }
    }

    public class LocationDTO
    {
        public string LocationUId { get; set; }
        public string LocationName { get; set; }
        public string Province { get; set; }
        public string City { get; set; }
        public string? District { get; set; }
        public string Address { get; set; }
        public float Longitude { get; set; }
        public float Latitude { get; set; }
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
        public string MusicMId { get; set; }
        public string MusicName { get; set; }
        public string Singer { get; set; }
        public string Album { get; set; }

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
        public string LikerUserId { get; set; }
        public string LikerUserName { get; set; }
        public string LikerUserNickName { get; set; }
        public string LikerUserAvatarUrl { get; set; }
        public DateTime LikeTime { get; set; }
        public LikeDTO(string likerUserId, string likerUserName,string likerUserNickeName, string likerUserAvatarUrl, DateTime likeTime)
        {
            LikerUserId = likerUserId;
            LikerUserName = likerUserName;
            LikerUserNickName=likerUserNickeName;
            LikerUserAvatarUrl = likerUserAvatarUrl;
            LikeTime = likeTime;
        }
    }

    public class TagSummaryDTO
    {
        public string TagId { get; set; }
        public string TagName { get; set; }
        public TagSummaryDTO(string tagId, string tagName)
        {
            TagId = tagId;
            TagName = tagName;
        }
    }

}
