namespace Innermost.MusicHub.Crawler.Parsers.Singer
{
    internal class SingerNameParser : DataParser
    {
        public override Task InitializeAsync()
        {
            AddRequiredValidator(@"y\.qq\.com/n/ryqq/singer/\w{14}");
            return Task.CompletedTask;
        }

        protected override async Task ParseAsync(DataFlowContext context)
        {
            var singerName = context.Selectable.XPath(".//h1[@class='data__name_txt']")?.Value;
            if (singerName is null)
            {
                await Task.CompletedTask;
                return;
            }

            var singerMid = context.Request.Properties["singerMid"];
            var region = context.Request.Properties["region"];
            var url = $"http://localhost:3200/getSingerDesc?singermid={singerMid}";

            var request = new Request(url, new Dictionary<string, object>
            {
                {"singerMid",singerMid },
                {"region",region },
                {"singerName",singerName }
            });
            context.AddFollowRequests(request);
        }
    }
}
