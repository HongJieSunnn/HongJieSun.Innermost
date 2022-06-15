namespace ReactApp.HttpAggregator.Models.LogLife
{
    public class RecommendedMusicRecord
    {
        public string Mid { get; set; }
        public string MusicName{get;set;}
        public string MusicAlbum{get;set;}
        public string MusicSinger{get;set;}
        public string MusicCoverUrl{get;set;}
        public RecommendedMusicRecord(string mid,string musicName,string musicAlbum,string musicSinger,string musicCoverUrl)
        {
            Mid= mid;
            MusicName= musicName;
            MusicAlbum= musicAlbum;
            MusicSinger= musicSinger;
            MusicCoverUrl= musicCoverUrl;
        }
    }
}
