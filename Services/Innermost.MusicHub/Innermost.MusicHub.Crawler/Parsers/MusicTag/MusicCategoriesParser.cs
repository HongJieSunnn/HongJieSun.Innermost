namespace Innermost.MusicHub.Crawler.Parsers.MusicTag
{
    internal class MusicCategoriesParser : DataParserMongoDB
    {
        public override Task InitializeAsync()
        {
            AddRequiredValidator(@"http://localhost:3200/getSongListCategories");
            return Task.CompletedTask;
        }

        protected override async Task ParseAsync(DataFlowContext context)
        {
            var categoryGroupNames = new[] { "流派", "主题", "心情", "场景" };
            var categoryGroupCounts = new[] { 16, 16, 9, 13 };
            var categoryIds = context.Selectable.SelectList(Selectors.JsonPath("$.response.data.categories.[2:].items.[*].categoryId")).Select(c => int.Parse(c.Value)).ToList();
            var categoryNames = context.Selectable.SelectList(Selectors.JsonPath("$.response.data.categories.[2:].items.[*].categoryName")).Select(c => c.Value).ToList();//short name for crawl

            var randbIndex = categoryNames.IndexOf("R&#38;B");
            categoryNames[randbIndex] = "R&B";

            var mcIndex = categoryNames.IndexOf("MC喊麦");
            categoryIds.RemoveAt(mcIndex);
            categoryNames.RemoveAt(mcIndex);

            List<CategoryEntity> categories = new List<CategoryEntity>(categoryIds.Count);

            int indexOfName = 0;
            for (int j = 0; j < 4; ++j)
            {
                for (int i = 0; i < categoryGroupCounts[j]; i++)
                {
                    var fullName = $"音乐:{categoryGroupNames[j]}:{categoryNames[indexOfName]}";

                    MusicTagStatics.CategoriesFullNameDictionary.Add(fullName, categoryNames[indexOfName]);

                    categories.Add(new CategoryEntity(categoryIds[indexOfName], fullName));
                    ++indexOfName;
                }
            }

            var requests = GetCategoryRequests(categories);

            context.AddFollowRequests(requests);

            if (_dbContext.Categories.CountDocuments(_ => true) == 0)
                await _dbContext.Categories.InsertManyAsync(categories);//To get categories under the empty properties while resend.
            if (_dbContext.MusicTags.CountDocuments(_ => true) == 0)
                await _dbContext.MusicTags.InsertManyAsync(categories.Select(c => new MusicTagEntity(null, c.CategoryName, new List<string>())));
        }

        private List<Request> GetCategoryRequests(List<CategoryEntity> categories)
        {
            var requests = categories.Select(c => new Request($"http://localhost:3200/getSongLists?categoryId={c.CategoryId}&limit=150", new Dictionary<string, object>
            {
                {"categoryName",c.CategoryName},
            })).ToList();

            return requests;
        }
    }
}
