using Grpc.Core;
using Innermost.Intelligence.API.Infrastructure.Dictionaries;
using Innermost.Intelligence.API.LifeRecord;
using Innermost.Intelligence.API.Services.Recommendations;
using Innermost.Intelligence.API.Services.TextAnalytics;

namespace Innermost.Intelligence.API.Grpc.Services
{
    public class IntelligenceLifeRecordRecommendationService : IntelligenceLifeRecordRecommendationGrpc.IntelligenceLifeRecordRecommendationGrpcBase
    {
        private readonly ITextAnalyticsService _textAnalyticsService;
        private readonly ILifeRecordRecommendationService _lifeRecordRecommendationService;
        public IntelligenceLifeRecordRecommendationService(ITextAnalyticsService textAnalyticsService, ILifeRecordRecommendationService lifeRecordRecommendationService)
        {
            _textAnalyticsService = textAnalyticsService;
            _lifeRecordRecommendationService = lifeRecordRecommendationService;
        }
        public override async Task<PredictedEmotionGrpcDTO> GetLifeRecordEmotion(LifeRecordTextGrpcDTO request, ServerCallContext context)
        {
            var sentimentDoc = await _textAnalyticsService.PredictSentimentAsync(request.LifeRecordText);

            var tag = EmotionTagDictionary.EmotionTags[sentimentDoc.Sentiment];

            return new PredictedEmotionGrpcDTO() { TagId = tag.TagId, TagName = tag.TagName };
        }

        public override async Task<LifeRecordRecomendationGrpcDTO> GetLifeRecordRecommendation(LifeRecordGrpcDTO request, ServerCallContext context)
        {
            var recommendation = await _lifeRecordRecommendationService.RecommendByEmotionAsync(request.PredictedEmotionTagName);

            return new LifeRecordRecomendationGrpcDTO()
            {
                Type = recommendation.Type,
                Content = recommendation.Content,
            };
        }
    }
}
