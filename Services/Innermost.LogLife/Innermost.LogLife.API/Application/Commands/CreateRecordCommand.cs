namespace Innermost.LogLife.API.Application.Commands
{
    public class CreateRecordCommand:IRequest<bool>
    {
        public string? Title { get; set; }
        public string Text { get; set; }
        public string? UserId { get; set; }
        public bool IsShared { get; set; }

        public string? LocationUId { get; set; }
        public string? LocationName { get; set; }
        public string? Province { get; set; }
        public string? City { get; set; }
        public string? District { get; set; }
        public string? Address { get; set; }
        public float? Longitude { get; set; }
        public float? Latitude { get; set; }

        public string? MusicId { get; set; }
        public string? MusicName { get; set; }
        public string? Singer { get; set; }
        public string? Album { get; set; }

        public List<string>? ImagePaths { get; set; }

        public DateTime CreateTime { get; set; }

        public Dictionary<string,string> TagSummaries { get; set; }

        public CreateRecordCommand(string userId, string? title, string text,bool isShared,
            string? locationUId, string? locationName, string? province, string? city, string? district, string? address, float? longitude, float? latitude,
            string? musicId, string? musicName, string? singer, string? album,
            List<string>? imagePaths,
            DateTime createTime,
            Dictionary<string, string> tagSummaries)
        {
            UserId=userId;
            Title=title;
            Text=text;
            IsShared=isShared;
            LocationUId=locationUId;
            LocationName=locationName;
            Province=province;
            City=city;
            District=district;
            Address=address;
            Longitude=longitude;
            Latitude=latitude;
            MusicId=musicId;
            MusicName=musicName;
            Singer=singer;
            Album=album;
            ImagePaths=imagePaths;
            CreateTime=createTime;
            TagSummaries=tagSummaries;
        }
    }
}
