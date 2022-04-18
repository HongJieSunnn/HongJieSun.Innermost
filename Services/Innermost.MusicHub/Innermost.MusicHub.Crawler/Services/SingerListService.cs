namespace Innermost.MusicHub.Crawler.Services
{
    internal class SingerListService
    {
        private static readonly CrawlerMongoDBContext _dbContext=DependencyInjection.ServiceProvider.GetRequiredService<CrawlerMongoDBContext>();
        public static async Task<List<Request>> GetRequestsBySingerListAsync(string url)
        {
            var singerList=await _dbContext.SingerList.FindAsync(_=>true);
            var requests = singerList.ToList().Select(s =>
            {
                var request = new Request(string.Format(url, s.SingerMid), new Dictionary<string, object> { { "region", s.Region }, { "singerMid", s.SingerMid } });
                request.Headers.UserAgent = UserAgentPool.GetRandomUserAgent();
                return request;
            }).ToList();
            return requests;
        }
    }
}
