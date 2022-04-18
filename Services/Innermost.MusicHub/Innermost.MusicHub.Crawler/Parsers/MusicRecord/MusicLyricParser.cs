namespace Innermost.MusicHub.Crawler.Parsers.MusicRecord
{
    internal class MusicLyricParser : DataParserMongoDB
    {
        public override Task InitializeAsync()
        {
            AddRequiredValidator(@"localhost:3200/getLyric\?songmid=\w{14}");
            return Task.CompletedTask;
        }

        protected override async Task ParseAsync(DataFlowContext context)
        {
            AlbumService.TakeOne();
            if (context.Response.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                Thread.Sleep(TimeSpan.FromMilliseconds(300));
                await Task.CompletedTask;
                return;
            }

            if (context.Response.StatusCode != System.Net.HttpStatusCode.OK)
            {
                Thread.Sleep(TimeSpan.FromSeconds(5));
            }
            var requestUri=context.Request.RequestUri.ToString();
            var mid = requestUri.Substring(requestUri.LastIndexOf("=") + 1, 14);

            var filter = Builders<MusicRecordEntity>.Filter.Eq(m => m.MusicMid, mid);
            var musicRecord = await _dbContext.MusicRecords.Find(filter).FirstAsync();
            var lyric = context.Selectable.JsonPath("$.response.lyric")?.Value;

            var update = Builders<MusicRecordEntity>.Update.Set(m => m.Lyric, lyric ?? "");
            await _dbContext.MusicRecords.UpdateOneAsync(filter, update);
        }
    }
}
