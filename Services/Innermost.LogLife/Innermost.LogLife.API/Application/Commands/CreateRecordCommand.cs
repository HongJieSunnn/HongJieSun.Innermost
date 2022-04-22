namespace Innermost.LogLife.API.Application.Commands
{
    public class CreateRecordCommand:IRequest<bool>
    {
        public string? Title { get; set; }

        [StringLength(3000,MinimumLength = 1)]
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

        public DateTime? CreateTime { get; set; }
        public Dictionary<string,string> TagSummaries { get; set; }

        //Tip:Nullable params in JsonConstructor can be not passed by json.But the type of params in constructor must be same as property or fields the params pass to.
        //For exsample,nullable params must pass to nullable properties or fields.
        /* Json under can deserialize successfully.
         * But if property CreateTime's type is DateTime but param createTime's type is DateTime?,even if all params have been passed,there is still throw exception:
         * Each parameter in the deserialization constructor on type 'Innermost.LogLife.API.Application.Commands.CreateRecordCommand' must bind to an object property or field on deserialization. 
         * Each parameter name must match with a property or field on the object. The match can be case-insensitive.
         * {
                "text":"油画一样的天空，你真该出现然后和我一块去看看的。",
                "isShared":false,
                "TagSummaries":{
                    "623c16ab3617e7aaf8343318":"心情:积极"
                }
            }
         */
        public CreateRecordCommand(string? userId, string? title, string text,bool isShared,
            string? locationUId, string? locationName, string? province, string? city, string? district, string? address, float? longitude, float? latitude,
            string? musicId, string? musicName, string? singer, string? album,
            List<string>? imagePaths,
            DateTime? createTime,
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
            CreateTime=createTime??DateTime.Now;
            TagSummaries=tagSummaries;
        }
    }
}
