using Newtonsoft.Json.Linq;

namespace Innermost.Intelligence.API.Services.DailySentence
{
    public class DailySentenceService : IDailySentenceService
    {
        private readonly IHttpClientFactory _httpClientFactory;

        private readonly Random _random;
        private const string DailySentenceUrl = "http://open.iciba.com/dsapi";
        public DailySentenceService(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory=httpClientFactory;
            _random = new Random();
        }
        public async Task<string> GetDailySentenceAsync()
        {
            var client=_httpClientFactory.CreateClient();

            var response =await client.GetAsync(DailySentenceUrl);
            if(response.IsSuccessStatusCode)
            {
                var content = new JObject(await response.Content.ReadAsStringAsync());

                var sentence = content["note"]?.ToString();

                if (sentence is not null)
                    return sentence;
            }
            return string.Empty;
        }

        public async Task<string> GetRandomDateDailySentenceAsync()
        {
            var client = _httpClientFactory.CreateClient();

            var randomDate = GenerateRandomDate();
            var randomDailySentenceUrl = $"http://sentence.iciba.com/index.php?c=dailysentence&m=getdetail&title={randomDate}";

            var response = await client.GetAsync(randomDailySentenceUrl);
            if (response.IsSuccessStatusCode)
            {
                var content = JObject.Parse(await response.Content.ReadAsStringAsync());

                var sentence= content["note"]?.ToString();

                if (sentence is not null)
                    return sentence;
            }
            return string.Empty;
        }

        private string GenerateRandomDate()
        {
            DateTime start = new DateTime(2016, 1, 1);
            int range = (DateTime.Today - start).Days;
            return start.AddDays(_random.Next(range)).ToString("yyyy-MM-dd");
        }
    }
}
