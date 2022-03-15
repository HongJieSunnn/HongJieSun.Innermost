namespace Innemost.LogLife.API.Queries.Model
{
    public record LifeRecordDTO
    {
        public int RecordId { get; set; }
        public string? Title { get; set; }
        public string Text { get; set; }
        public bool IsShared { get; set; }

        [JsonPropertyName("Location")]
        public LocationDTO? Location { get; set; }

        [JsonPropertyName("MusicRecord")]
        public MusicRecordDTO? MusicRecord { get; set; }

        public List<string>? ImagePaths { get; set; }

        public DateTime CreateTime { get; set; }

        public List<TagSummaryDTO> TagSummaries { get; set; }

        public LifeRecordDTO(int recordId,string? title,string text,bool isShared,LocationDTO? locationDTO,MusicRecordDTO? musicRecordDTO,List<string>? imagePaths,DateTime createTime, List<TagSummaryDTO> tagSummaries)
        {
            RecordId=recordId;
            Title=title;
            Text=text;
            IsShared=isShared;
            Location=locationDTO;
            MusicRecord=musicRecordDTO;
            CreateTime=createTime;
            ImagePaths=imagePaths;
            TagSummaries=tagSummaries;
        }
    }

    public record LocationDTO
    {
        public string LocationUId { get; set; }
        public string LocationName { get; set; }
        public string Province { get; set; }
        public string City { get; set; }
        public string District { get; set; }
        public string Address { get; set; }
        public float Longitude { get; set; }
        public float Latitude { get; set; }
        public LocationDTO(string locationUId,string locationName,string province,string city,string district,string address,float longitude,float latitue)
        {
            LocationUId=locationUId;
            LocationName=locationName;
            Province=province;
            City=city;
            District=district;
            Address=address;
            Longitude=longitude;
            Latitude = latitue;
        }
    }

    public record MusicRecordDTO
    {
        public string MusicId { get; set; }
        public string MusicName { get; set; }
        public string Singer { get; set; }
        public string Album { get; set; }
        public MusicRecordDTO(string musicId,string musicName,string singer,string album)
        {
            MusicId=musicId;
            MusicName=musicName;
            Singer=singer;
            Album=album;
        }
    }

    public record TagSummaryDTO
    {
        public string TagId { get; set; }
        public string TagName { get; set; }
        public TagSummaryDTO(string tagId,string tagName)
        {
            TagId=tagId;
            TagName=tagName;
        }
    }
}
