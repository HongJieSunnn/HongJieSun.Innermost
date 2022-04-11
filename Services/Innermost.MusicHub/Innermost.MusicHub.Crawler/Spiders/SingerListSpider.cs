namespace Innermost.MusicHub.Crawler.Spiders
{
    internal class SingerListSpider : Spider
    {
        private int _genre = -100;
        public List<Request> SingerListRequests { get; set; }
        public Dictionary<string, int> MaxSingerCountForEachRegion => new Dictionary<string, int>()
        {
            {"内地",0 },
            {"港台",1 },
            {"欧美",0 },
            {"日本",0 },
            {"韩国",0 },
            {"其它",0 },
        };
        public Dictionary<string, int> Regions => new Dictionary<string, int>()
        {
            {"内地",200 },
            {"港台",2 },
            {"欧美",5 },
            {"日本",4 },
            {"韩国",3 },
            {"其它",6 },
        };

        public SingerListSpider(IOptions<SpiderOptions> options, DependenceServices services, ILogger<Spider> logger) : base(options, services, logger)
        {
            SingerListRequests = GetSingerListRequestsAsync();
        }

        private List<Request> GetSingerListRequestsAsync()
        {
            List<Request> singerListRequests = new List<Request>();
            foreach (var region in Regions)
            {
                int regionCode = region.Value;
                string regionName = region.Key;
                int currentPage = 1;
                int currentIndex = 0;
                int maxSingerCountForNowRegion = MaxSingerCountForEachRegion[regionName];
                do
                {
                    var requestUrl = "https://u.y.qq.com/cgi-bin/musicu.fcg?" +
                                            "data=%7B%22comm%22%3A%7B%22ct%22%3A24%2C%22cv%22%3A0%7D%2C%22singerList%22%3A%7B%22module%22%3A%22" +
                                            "Music.SingerListServer%22%2C%22method%22%3A%22get_singer_list%22%2C%22param%22%3A%7B%22area%22%3A" +
                                            $"{regionCode}%2C%22sex%22%3A-100%2C%22genre%22%3A" +
                                            $"{_genre}%2C%22index%22%3A-100%2C%22sin%22%3A" +
                                            $"{currentIndex}%2C%22cur_page%22%3A" +
                                            $"{currentPage}%7D%7D%7D";
                    var properties = new Dictionary<string, object>()
                    {
                        {"SingerCountToTake",((maxSingerCountForNowRegion-currentIndex)>=80)?80:maxSingerCountForNowRegion-currentIndex },
                        {"Region",regionName }
                    };
                    var request = new Request(requestUrl, properties);
                    singerListRequests.Add(request);

                    ++currentPage;
                    currentIndex = 80 * (currentPage - 1);
                } while (currentIndex < MaxSingerCountForEachRegion[regionName]);
            }

            return singerListRequests;
        }

        protected override async Task InitializeAsync(CancellationToken stoppingToken = default)
        {
            AddDataFlow(new SingerListParser());
            AddDataFlow(new SingerListStorage());
            await AddRequestsAsync(SingerListRequests);
        }
    }
}
