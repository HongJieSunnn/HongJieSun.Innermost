namespace Innermost.MusicHub.Crawler.Parsers.Singer
{
    internal class SingerDetailParser : DataParser
    {
        private readonly IHttpClientFactory _httpClientFactory;
        public SingerDetailParser()
        {
            _httpClientFactory = DependencyInjection.ServiceProvider.GetRequiredService<IHttpClientFactory>();
        }
        public override Task InitializeAsync()
        {
            AddRequiredValidator(@"localhost:3200/getSingerDesc\?singermid=\w{14}");
            return Task.CompletedTask;
        }

        protected override async Task ParseAsync(DataFlowContext context)
        {
            var singerMid=(string)context.Request.Properties["singerMid"];
            var singerRegion = (string)context.Request.Properties["region"];

            var resultXml = context.Selectable.JsonPath("$.response");
            var resultXmlId = resultXml.XPath(".//id").Value;

            var client = _httpClientFactory.CreateClient();
            var coverResponseTask = client.GetAsync($"http://imgcache.qq.com/music/photo/singer_800/20/800_singerpic_{resultXmlId}_0.jpg");//get singer cover

            var resultXmlDesc = RemoveCDATAPattern((resultXml.XPath(".//desc") as HtmlSelectable)!.InnerHtml);
            var resultXmlItems = resultXml.SelectList(Selectors.XPath("//item")).ToDictionary(
                keySelector: i => RemoveCDATAPattern((i.XPath(".//key") as HtmlSelectable)!.InnerHtml),
                elementSelector:i=>RemoveCDATAPattern((i.XPath(".//value") as HtmlSelectable)!.InnerHtml)
            );//Convert singer info items to dictionary.

            var singerName = (string)context.Request.Properties["singerName"];
            var singerAlias = GetSingerAlias(resultXmlItems);
            var singerNationality = resultXmlItems["国籍"];
            var singerBirthPlace = resultXmlItems["出生地"];
            var singerOccupation = resultXmlItems["职业"];
            var singerBirthday = resultXmlItems["出生日期"];
            var singerRepresentativeWorks= resultXmlItems["代表作品"];

            var converResponse = await coverResponseTask;
            var coverUrl = converResponse!.RequestMessage!.RequestUri!.ToString();//the singer cover will be responsed by a new uri which can be got by HttpResponseMessage.RequestMessage

            var singer = new SingerEntity(singerMid, int.Parse(resultXmlId), singerName, singerAlias, singerNationality, singerBirthPlace, singerOccupation, singerBirthday, singerRepresentativeWorks, singerRegion, coverUrl);
            context.AddData("singer", singer);
        }

        private string RemoveCDATAPattern(string xmlString)
        {
            return xmlString.Replace("<![CDATA[", "").Replace("]]>", "");
        }

        private string GetSingerAlias(Dictionary<string,string> resultXmlItems)
        {
            var singerAlias = string.Empty;
            if (resultXmlItems.ContainsKey("外文名"))
                singerAlias = resultXmlItems["外文名"];
            if(resultXmlItems.ContainsKey("中文名"))
                singerAlias= resultXmlItems["中文名"];

            return singerAlias;
        }
    }
}
