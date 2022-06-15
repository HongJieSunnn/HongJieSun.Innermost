namespace Innermost.MusicHub.Crawler.Parsers
{
    internal class SingerListParser : DataParser
    {
        public override Task InitializeAsync()
        {
            return Task.CompletedTask;
        }

        protected override Task ParseAsync(DataFlowContext context)
        {
            var singerCountToTake = (int)context.Request.Properties["SingerCountToTake"];//TODO:I don't know why the count of properties will be 0 while resending.By reading source code,it should not be.
            var region = (string)context.Request.Properties["Region"];

            var singerList = context.Selectable.SelectList(Selectors.JsonPath("$.singerList.data.singerlist.[*].singer_mid")).Select(s => new SingerListEntity(s.Value, region)).Skip(0).Take(singerCountToTake);
            context.AddData("singerList", singerList);

            return Task.CompletedTask;
        }
    }
}
