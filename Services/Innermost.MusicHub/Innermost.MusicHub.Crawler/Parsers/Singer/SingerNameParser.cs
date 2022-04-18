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
            var singerName = context.Selectable.XPath(".//h1[@class='data__name_txt']").Value;

            var url = "http://localhost:3200/getSingerDesc?singermid={0}";
            var requests = await SingerListService.GetRequestsBySingerListAsync(url);
            for(int i = 0; i < requests.Count; ++i)
            {
                requests[i].Properties.Add("singerName", singerName);
            }
            context.AddFollowRequests(requests);
        }
    }
}
