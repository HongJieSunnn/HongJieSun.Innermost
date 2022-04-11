namespace Innermost.MusicHub.Crawler.Storages.SingerList
{
    internal class SingerListStorage : DataFlowMongoDB
    {
        public override async Task HandleAsync(DataFlowContext context)
        {
            IEnumerable<SingerListEntity> singerList = context.GetData("singerList");
            if(singerList is not null&&singerList.Count()>0)
                await _dbContext.SingerList.InsertManyAsync(singerList);

            SpiderRunner.SingerListBarrier.SignalAndWait();
        }

        public override Task InitializeAsync()
        {
            return Task.CompletedTask;
        }
    }
}
