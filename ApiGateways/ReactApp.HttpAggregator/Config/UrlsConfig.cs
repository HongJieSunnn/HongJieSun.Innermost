namespace ReactApp.HttpAggregator.Config
{
    public class UrlsConfig
    {
        public string? Fileserver{get;set;}
        public string? Identity{get;set;}
        public string? Intelligence{get;set;}
        public string? Loglife{get;set;}
        public string? Meet{get;set;}
        public string? MeetSignalRHub{get;set;}
        public string? MusicHub{get;set;}
        public string? Push{get;set;}
        public string? Tagserver { get; set; }

        public class LogLifeOperations
        {
            public static string CreateLifeRecord() => "/api/loglife/create";
        }
    }
}
