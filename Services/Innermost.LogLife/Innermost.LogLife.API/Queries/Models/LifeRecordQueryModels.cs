namespace Innemost.LogLife.API.Queries.Model
{
    public record LifeRecordDTO
    {
        public int RecordId { get; init; }
        public string? Title { get; init; }
        public string Text { get; init; }
        public bool IsShared { get; init; }

        [JsonPropertyName("Location")]
        public LocationDTO? Location { get; init; }

        [JsonPropertyName("MusicRecord")]
        public MusicRecordDTO? MusicRecord { get; init; }

        public List<string>? ImagePaths { get; init; }

        public DateTime CreateTime { get; init; }

        public List<TagSummaryDTO> TagSummaries { get; init; }

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
        public string LocationUId { get; init; }
        public string LocationName { get; init; }
        public string Province { get; init; }
        public string City { get; init; }
        public string District { get; init; }
        public string Address { get; init; }
        public float Longitude { get; init; }
        public float Latitude { get; init; }
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
        public string MusicId { get; init; }
        public string MusicName { get; init; }
        public string Singer { get; init; }
        public string Album { get; init; }
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
        public string TagId { get; init; }
        public string TagName { get; init; }
        public TagSummaryDTO(string tagId,string tagName)
        {
            TagId=tagId;
            TagName=tagName;
        }
    }
}
