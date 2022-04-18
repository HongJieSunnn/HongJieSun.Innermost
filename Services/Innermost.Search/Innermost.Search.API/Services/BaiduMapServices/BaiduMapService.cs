namespace Innermost.Search.API.Services.BaiduMapServices
{
    public class BaiduMapService:IBaiduMapService
    {
        private readonly IConfiguration _configuration;
        private readonly IHttpClientFactory _httpClientFactory;
        public BaiduMapService(IConfiguration configuration,IHttpClientFactory httpClientFactory)
        {
            _configuration=configuration;
            _httpClientFactory=httpClientFactory;
        }

    }
}
