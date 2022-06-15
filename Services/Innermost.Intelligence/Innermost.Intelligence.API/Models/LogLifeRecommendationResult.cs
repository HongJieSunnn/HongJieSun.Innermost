namespace Innermost.Intelligence.API.Models
{
    public class LogLifeRecommendationResult
    {
        public string Type { get; init; }
        public string Content { get; init; }
        public LogLifeRecommendationResult(string type,string content)
        {
            Type = type;
            Content = content;
        }
    }
}
