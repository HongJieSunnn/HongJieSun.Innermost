using Newtonsoft.Json.Linq;
using ReactApp.HttpAggregator.Models.Daily;

namespace ReactApp.HttpAggregator.Services.Daily
{
    public class DailyService:IDailyService
    {
        private readonly HttpClient _client;

        private const string BingUrlPrefix = "https://www.bing.com";
        public DailyService(HttpClient httpClient)
        {
            _client= httpClient;
        }

        public async Task<DailyPictureModel?> GetDailyPictureAsync()
        {
            var res=await _client.GetAsync("https://www.bing.com/HPImageArchive.aspx?format=js&idx=0&n=1");

            if (res.IsSuccessStatusCode)
            {
                var json = JObject.Parse(await res.Content.ReadAsStringAsync())["images"]?[0];
                if(json is null)
                    return null;

                var title= json["title"]!.ToString();
                var url = $"{BingUrlPrefix}{json["url"]}";
                var copyright= json["copyright"]!.ToString();
                var copyrightLink= json["copyrightlink"]!.ToString();

                return new DailyPictureModel(title,url,copyright,copyrightLink);
            }

            return null;
        }

        public async Task<string> GetDailySentenceAsync()
        {
            var res = await _client.GetAsync("http://open.iciba.com/dsapi/");

            if(res.IsSuccessStatusCode)
            {
                var json=JObject.Parse(await res.Content.ReadAsStringAsync());

                return json["note"]!.ToString();
            }

            return "祝好心情";
        }
    }
}
