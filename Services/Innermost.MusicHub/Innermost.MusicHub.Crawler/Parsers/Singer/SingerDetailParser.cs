namespace Innermost.MusicHub.Crawler.Parsers.Singer
{
    internal class SingerDetailParser : DataParser
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly CrawlerMongoDBContext _dbContext;
        public SingerDetailParser()
        {
            _httpClientFactory = DependencyInjection.ServiceProvider.GetRequiredService<IHttpClientFactory>();
            _dbContext = DependencyInjection.ServiceProvider.GetRequiredService<CrawlerMongoDBContext>();
        }
        public override Task InitializeAsync()
        {
            AddRequiredValidator(@"localhost:3200/getSingerDesc\?singermid=\w{14}");
            return Task.CompletedTask;
        }

        protected override async Task ParseAsync(DataFlowContext context)
        {
            context.Request.Properties.TryGetValue("singerMid", out var singerMid);
            context.Request.Properties.TryGetValue("region", out var singerRegion);
            context.Request.Properties.TryGetValue("singerName", out var singerName);
            //properties' count will 0 after resend and I dont know why so we get properties if the properties' count in request is 0.
            if (singerMid is null)
            {
                var requestUri = context.Request.RequestUri.ToString();
                var mid = requestUri.Substring(requestUri.LastIndexOf("=") + 1, 14);

                var eqFilter = Builders<SingerListEntity>.Filter.Eq("_id", mid);
                var singerEntity = (await _dbContext.SingerList.FindAsync(eqFilter)).First();

                singerMid = singerEntity.SingerMid;
                singerRegion = singerEntity.Region;
            }

            if (singerName is null)
            {
                var singerNameClient = _httpClientFactory.CreateClient();
                var res = await singerNameClient.GetAsync($"https://y.qq.com/n/ryqq/singer/{singerMid}");
                var singerNameSelectable = new HtmlSelectable(await res.Content.ReadAsStringAsync());
                singerName = singerNameSelectable.XPath(".//h1[@class='data__name_txt']")?.Value;
                if (singerName is null)
                {
                    await Task.CompletedTask;
                    return;
                }
            }

            var resultXml = context.Selectable.JsonPath("$.response");
            var resultXmlId = resultXml.XPath(".//id")?.Value;

            if (resultXmlId is null)
            {
                await Task.CompletedTask;
                return;
            }

            var client = _httpClientFactory.CreateClient();
            var coverResponseTask = client.GetAsync($"http://imgcache.qq.com/music/photo/singer_800/20/800_singerpic_{resultXmlId}_0.jpg");//get singer cover

            var resultXmlDesc = RemoveCDATAPattern((resultXml.XPath(".//desc") as HtmlSelectable)?.InnerHtml);

            var basicItems = resultXml.SelectList(Selectors.XPath(".//basic/item"));
            Dictionary<string, string> resultXmlItems = new Dictionary<string, string>();
            if (basicItems is not null && basicItems.Count() > 0)//very few situation that the singer's desc info will contain duplicate key.Like 003MttY107zGjx,004CY5el0Sce5I under 5000 singers.
                resultXmlItems = basicItems.ToDictionary(
                    keySelector: i => RemoveCDATAPattern((i.XPath(".//key") as HtmlSelectable)!.InnerHtml),
                    elementSelector: i => RemoveCDATAPattern((i.XPath(".//value") as HtmlSelectable)!.InnerHtml)
                );//Convert singer info items to dictionary.

            var singerAlias = GetSingerAlias(resultXmlItems);
            var singerNationality = resultXmlItems.GetValueOrDefault("国籍", "");
            var singerBirthPlace = resultXmlItems.GetValueOrDefault("出生地", "");
            var singerOccupation = resultXmlItems.GetValueOrDefault("职业", "");
            var singerBirthday = resultXmlItems.GetValueOrDefault("出生日期", "");
            var singerRepresentativeWorks = resultXmlItems.GetValueOrDefault("代表作品", "");

            var converResponse = await coverResponseTask;
            var coverUrl = converResponse!.RequestMessage!.RequestUri!.ToString();//the singer cover will be responsed by a new uri which can be got by HttpResponseMessage.RequestMessage

            var singer = new SingerEntity((string)singerMid, int.Parse(resultXmlId), (string)singerName, singerAlias, singerNationality, singerBirthPlace, singerOccupation, singerBirthday, singerRepresentativeWorks, (string)singerRegion!, coverUrl);
            context.AddData("singer", singer);
        }

        private string RemoveCDATAPattern(string? xmlString)
        {
            if (xmlString is null)
                return "";
            return xmlString.Replace("<![CDATA[", "").Replace("]]>", "");
        }

        private string GetSingerAlias(Dictionary<string, string> resultXmlItems)
        {
            var singerAlias = string.Empty;
            if (resultXmlItems.ContainsKey("外文名"))
                singerAlias = resultXmlItems["外文名"];
            if (resultXmlItems.ContainsKey("中文名"))
                singerAlias = resultXmlItems["中文名"];

            return singerAlias;
        }
    }
}
