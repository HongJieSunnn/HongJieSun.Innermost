namespace Innermost.MusicHub.Crawler.Storages.Album
{
    internal class AlbumStorage : DataFlowMongoDB
    {
        public override async Task HandleAsync(DataFlowContext context)
        {
            var albums = context.GetData("album");
            if(albums is not null)
                await _dbContext.Albums.InsertOneAsync(albums);
            await Task.CompletedTask;
        }

        public override Task InitializeAsync()
        {
            return Task.CompletedTask;
        }
    }
}
