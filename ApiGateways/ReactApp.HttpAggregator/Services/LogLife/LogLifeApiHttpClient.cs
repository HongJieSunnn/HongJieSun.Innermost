namespace ReactApp.HttpAggregator.Services.LogLife
{
    public class LogLifeApiHttpClient:ILogLifeApiHttpClient
    {
        private readonly HttpClient _client;
        private readonly ILogger<LogLifeApiHttpClient> _logger;
        private readonly UrlsConfig _urls;
        public LogLifeApiHttpClient(HttpClient httpClient, ILogger<LogLifeApiHttpClient> logger, IOptions<UrlsConfig> config)
        {
            _client = httpClient;
            _logger = logger;
            _urls = config.Value;
        }

        public async Task<bool> CreteLifeRecordAsync(LifeRecordModel lifeRecordModel)
        {
            var url = $"{_urls.Loglife}{UrlsConfig.LogLifeOperations.CreateLifeRecord()}";
            var content = new StringContent(JsonSerializer.Serialize(lifeRecordModel), System.Text.Encoding.UTF8, "application/json");
            var response = await _client.PostAsync(url, content);

            if(response.IsSuccessStatusCode)
                return true;

            return false;
        }
    }
}
