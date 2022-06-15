namespace Innermost.Meet.API.Queries.StatueQueries.Models
{
    public class StatueDTO
    {
        public bool OnlineStatue { get; set; }
        public string UserStatue { get; set; }
        public StatueDTO(bool onlineStatue,string userStatue)
        {
            OnlineStatue = onlineStatue;
            UserStatue = userStatue;
        }
    }
}
