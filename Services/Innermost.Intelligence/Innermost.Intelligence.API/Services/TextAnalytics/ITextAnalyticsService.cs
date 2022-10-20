namespace Innermost.Intelligence.API.Services.TextAnalytics
{
    public interface ITextAnalyticsService
    {
        Task<DocumentSentiment> PredictSentimentAsync(string text, string language = "zh");
    }
}
