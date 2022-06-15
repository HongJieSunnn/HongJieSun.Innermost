

namespace Innermost.Intelligence.API.Services.Recommendations
{
    public interface ILifeRecordRecommendationService
    {
        Task<LogLifeRecommendationResult> RecommendByEmotionAsync(string predictedEmotionTagName);
    }
}
