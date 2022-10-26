namespace ReactApp.HttpAggregator.Models.Daily
{
    public class DailyPictureModel
    {
        public string Title { get; set; }
        public string PictureUrl { get; set; }
        public string Copyright { get; set; }
        public string CopyrightLink { get; set; }
        public DailyPictureModel(string title, string pictureUrl, string copyright, string copyrightLink)
        {
            Title = title;
            PictureUrl = pictureUrl;
            Copyright = copyright;
            CopyrightLink = copyrightLink;
        }
    }
}
