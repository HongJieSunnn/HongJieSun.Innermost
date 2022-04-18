namespace Innermost.MusicHub.Crawler.Parsers.MusicTag
{
    internal class MusicListDetailParser : DataParserMongoDB
    {
        public override Task InitializeAsync()
        {
            AddRequiredValidator(@"http://localhost:3200/getSongListDetail\?disstid=\d+");
            return Task.CompletedTask;
        }

        protected override async Task ParseAsync(DataFlowContext context)
        {
            var tags = context.Selectable.SelectList(Selectors.JsonPath("$.response.cdlist.[0].tags.[*].name")).Select(n => n.Value).Where(n => MusicTagStatics.CategoriesNameSet.Contains(n));
            var songMids= context.Selectable.SelectList(Selectors.JsonPath("$.response.cdlist.[0].songlist.[*].mid")).Select(n => n.Value);

            foreach (var tag in tags)
            {
                var tagFilter=Builders<MusicTagEntity>.Filter.Eq(m=>m.TagName,tag);
                var update = Builders<MusicTagEntity>.Update.AddToSetEach("MusicRecordMids", songMids);
                await _dbContext.MusicTags.UpdateManyAsync(tagFilter, update);
            }
        }
    }
}
