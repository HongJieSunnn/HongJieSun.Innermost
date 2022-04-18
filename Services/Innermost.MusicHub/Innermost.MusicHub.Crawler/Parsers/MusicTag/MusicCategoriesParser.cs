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
            var categoryIds = context.Selectable.SelectList(Selectors.JsonPath("$.response.data.categories.[2:5].items.[*].categoryId")).Select(c=>int.Parse(c.Value)).ToList();
            var categoryNames = context.Selectable.SelectList(Selectors.JsonPath("$.response.data.categories.[2:5].items.[*].categoryName")).Select(c => c.Value).ToList();

            var randbIndex = categoryNames.IndexOf("R&#38;B");
            categoryNames[randbIndex] = "R&B";

            var mcIndex= categoryNames.IndexOf("MC喊麦");
            categoryIds.RemoveAt(mcIndex);
            categoryNames.RemoveAt(mcIndex);

            List<CategoryEntity> categories = new List<CategoryEntity>(categoryIds.Count);

            for (int i = 0; i < categoryIds.Count; i++)
            {
                categories.Add(new CategoryEntity(categoryIds[i], categoryNames[i]));
            }

            MusicTagStatics.CategoriesNameSet=new HashSet<string>(categories.Select(c=>c.CategoryName));

            var requests = GetCategoryRequests(categories);

            context.AddFollowRequests(requests);

            if (_dbContext.Categories.CountDocuments(_ => true) == 0)
                await _dbContext.Categories.InsertManyAsync(categories);//To get categories under the empty properties while resend.
            if (_dbContext.MusicTags.CountDocuments(_ => true) == 0)
                await _dbContext.MusicTags.InsertManyAsync(categories.Select(c=> new MusicTagEntity(null,c.CategoryName,new List<string>())));
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
