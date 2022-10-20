namespace Innermost.Intelligence.API.Services.TextAnalytics
{
    public class TextAnalyticsService : ITextAnalyticsService
    {
        private readonly TextAnalyticsClient _textAnalyticsClient;
        public TextAnalyticsService(TextAnalyticsClient textAnalyticsClient)
        {
            _textAnalyticsClient = textAnalyticsClient;
        }

        public async Task<DocumentSentiment> PredictSentimentAsync(string text, string language = "zh")
        {
            var sentimentDoc = (await _textAnalyticsClient.AnalyzeSentimentAsync(text, language)).Value;

            return sentimentDoc;
        }
    }
}
