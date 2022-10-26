namespace ReactApp.HttpAggregator.Services.Intelligence
{
    public interface ILifeRecordIntelligenceService
    {
        Task<(string tagId, string tagName)> GetLifeRecordEmotionTagAsync(string text);

        Task<(string type, string content)> GetLifeRecordRecommendationAsync(string predictedTagName);
    }
}
