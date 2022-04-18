namespace Innermost.MusicHub.Crawler.Parsers.MusicTag
{
    internal class MusicListParser : DataParserMongoDB
    {
        public override Task InitializeAsync()
        {
            AddRequiredValidator(@"http://localhost:3200/getSongLists\?categoryId=\d+&limit=150");
            return Task.CompletedTask;
        }

        protected override async Task ParseAsync(DataFlowContext context)
        {
            var musicLists = context.Selectable.SelectList(Selectors.JsonPath("$.response.data.list.[*].dissid")).Select(m => new MusicListEntity(m.Value));
            var exitedMusicLists = (await _dbContext.MusicLists.FindAsync(_ => true)).ToList();

            var newMusicLists = musicLists.Except(exitedMusicLists);
            if (newMusicLists.Any())
                await _dbContext.MusicLists.InsertManyAsync(newMusicLists);


            context.AddFollowRequests(newMusicLists.Select(ml => new Request($"http://localhost:3200/getSongListDetail?disstid={ml.Dissid}")));
        }
    }
}
