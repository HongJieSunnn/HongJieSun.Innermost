namespace Innermost.Search.API.Models.BaiduMapModels
{
    public class BaiduMapLocatedByIPResponseDTO
    {
        public string Province { get; set; }
        public string City { get; set; }
        public BaiduMapLocatedByIPResponseDTO(string province,string city)
        {
            Province = province;
            City = city;
        }
    }
}
