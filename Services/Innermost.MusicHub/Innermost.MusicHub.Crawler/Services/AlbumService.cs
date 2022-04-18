namespace Innermost.MusicHub.Crawler.Services
{
    internal class AlbumService
    {
        private static readonly CrawlerMongoDBContext _dbContext = DependencyInjection.ServiceProvider.GetRequiredService<CrawlerMongoDBContext>();
        private static long _count = _dbContext.Albums.CountDocuments(_=>true);
        private static int _toTakeAlbumCount = 5000;
        private static int _musicCount = 0;
        public static int _offset = 0;
        private static int _takedCount = 0;
        private static object _lock = new object();
        
        public static AutoResetEvent CanTake=new AutoResetEvent(true);

        public static async Task<List<Request>> GetRequestsByAlbumSongsAsync(string url= "http://localhost:3200/getSongInfo?songmid={0}")
        {
            var updateTime = Builders<StartMusicRecordHostTime>.Update.Inc(t => t.Times, 1);
            _dbContext.StartMusicRecordHostTime.UpdateOne(t => t.Id == 1, updateTime);
            var albums = await _dbContext.Albums.Find(_ => true).Skip(_offset).Limit(_toTakeAlbumCount).ToListAsync();
            var musicMids= albums.SelectMany(a=>a.MusicRecordMids);
            _musicCount = musicMids.Count();

            var requests = albums.SelectMany(s =>
            {
                var requests = s.MusicRecordMids.Select(m => new Request(string.Format(url, m), new Dictionary<string, object> { { "albumCoverUrl", s.AlbumCoverUrl! } })).ToList();
                for(int i=0;i<requests.Count;i++)
                {
                    requests[i].Headers.UserAgent = UserAgentPool.GetRandomUserAgent();
                }
                return requests;
            }).ToList();

            _offset += _toTakeAlbumCount;

            return requests;
        }

        public static void TakeOne()
        {
            lock (_lock)
            {
                _takedCount++;
                if(_takedCount==2500)
                {
                    Thread.Sleep(TimeSpan.FromSeconds(15));
                    _takedCount = 0;
                }
                //if (_takedCount==23456)
                //{
                //    _takedCount = 0;
                //    CanTake.Set();
                //}
            }
        }

        public static bool IsFinished()
        {
            return _offset >= _count;
        }

        public static bool IsReadToFinish()
        {
            return _offset == _count-1;
        }

        public static async Task WaitForNextRun()
        {
            if(_offset!=0)
                await Task.Delay(TimeSpan.FromSeconds(5));
        }
    }
}
