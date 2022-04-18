namespace Innermost.MusicHub.Crawler.Storages.MusicRecord
{
    internal class MusicRecordStorage : DataFlowMongoDB
    {
        public override async Task HandleAsync(DataFlowContext context)
        {
            var musicRecord=context.GetData("musicRecord");
            if(musicRecord is not null)
                await _dbContext.MusicRecords.InsertOneAsync(musicRecord);
            await Task.CompletedTask;
        }

        public override Task InitializeAsync()
        {
            return Task.CompletedTask;
        }
    }
}
