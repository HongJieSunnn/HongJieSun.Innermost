using Innermost.Intelligence.API.LifeRecord;

namespace ReactApp.HttpAggregator.Services.Intelligence
{
    public class LifeRecordIntelligenceService : ILifeRecordIntelligenceService
    {
        private readonly IntelligenceLifeRecordRecommendationGrpc.IntelligenceLifeRecordRecommendationGrpcClient _intelligenceLifeRecordRecommendationGrpcClient;
        public LifeRecordIntelligenceService(IntelligenceLifeRecordRecommendationGrpc.IntelligenceLifeRecordRecommendationGrpcClient intelligenceLifeRecordRecommendationGrpcClient)
        {
            _intelligenceLifeRecordRecommendationGrpcClient=intelligenceLifeRecordRecommendationGrpcClient;
        }
        public async Task<(string tagId, string tagName)> GetLifeRecordEmotionTagAsync(string text)
        {
            var tag = await _intelligenceLifeRecordRecommendationGrpcClient.GetLifeRecordEmotionAsync(new LifeRecordTextGrpcDTO() { LifeRecordText = text });

            return (tag.TagId, tag.TagName);
        }

        public async Task<(string type, string content)> GetLifeRecordRecommendationAsync(string predictedTagName)
        {
            var recommendation = await _intelligenceLifeRecordRecommendationGrpcClient.GetLifeRecordRecommendationAsync(new LifeRecordGrpcDTO() { PredictedEmotionTagName = predictedTagName });

            return (recommendation.Type, recommendation.Content);
        }
    }
}
