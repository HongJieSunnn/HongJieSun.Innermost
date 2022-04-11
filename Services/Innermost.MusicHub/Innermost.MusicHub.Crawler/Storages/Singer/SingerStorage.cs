namespace Innermost.MusicHub.Crawler.Storages.Singer
{
    internal class SingerStorage : DataFlowMongoDB
    {
        public override async Task HandleAsync(DataFlowContext context)
        {
            var singer = context.GetData("singer");
            if (singer is not null)
                await _dbContext.Singers.InsertOneAsync(singer);
            await Task.CompletedTask;
        }

        public override Task InitializeAsync()
        {
            return Task.CompletedTask;
        }
    }
}
