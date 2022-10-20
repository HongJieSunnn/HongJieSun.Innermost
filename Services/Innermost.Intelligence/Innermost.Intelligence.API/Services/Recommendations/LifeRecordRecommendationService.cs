using Innermost.Intelligence.API.Infrastructure.Dictionaries;
using Innermost.Intelligence.API.Services.DailySentence;
using System.Text.Json;

namespace Innermost.Intelligence.API.Services.Recommendations
{
    public class LifeRecordRecommendationService : ILifeRecordRecommendationService
    {
        private readonly IDailySentenceService _dailySentenceService;
        private readonly MusicRecordGrpc.MusicRecordGrpcClient _musicRecordGrpcClient;

        private readonly Random _random;
        private readonly Dictionary<string, Func<Task<LogLifeRecommendationResult>>> _negativeEmotionRecommendationFunctions;
        private const string mixedRecommandationReulstContent = "很少有记录对应的情绪会被系统判断为 mixed ，我们相信，留下这段记录的你一定是一个具有深度思想的人，希望你能尽情遨游在自己的思想海洋中，希望你能慢慢探索至你的 Innermost。";
        public LifeRecordRecommendationService(IDailySentenceService dailySentenceService, MusicRecordGrpc.MusicRecordGrpcClient musicRecordGrpcClient)
        {
            _dailySentenceService = dailySentenceService;
            _musicRecordGrpcClient = musicRecordGrpcClient;

            _random = new Random();
            _negativeEmotionRecommendationFunctions = new Dictionary<string, Func<Task<LogLifeRecommendationResult>>>
            {
                { "NegativeEmotionMusicRecommendationResult",GetNegativeEmotionMusicRecommendationAsync},
            };
        }

        public Task<LogLifeRecommendationResult> RecommendByEmotionAsync(string predictedEmotionTagName)
        {
            switch (predictedEmotionTagName)
            {
                case TagSummary.PositiveTagName:
                case TagSummary.NeutralTagName:
                    return GetPositiveEmotionRecommendationAsync();
                case TagSummary.NegativeTagName:
                    return GetNegativeEmotionRecommendationAsync();
                case TagSummary.MixedTagName:
                    return GetMixedEmotionRecommendationAsync();
            }
            return GetPositiveEmotionRecommendationAsync();
        }

        private async Task<LogLifeRecommendationResult> GetPositiveEmotionRecommendationAsync()
        {
            var dailySentence = await _dailySentenceService.GetRandomDateDailySentenceAsync();
            return new LogLifeRecommendationResult("PositiveEmotionRecommendationResult", dailySentence);
        }

        private Task<LogLifeRecommendationResult> GetNegativeEmotionRecommendationAsync()
        {
            var randomRecommendationFunction = _negativeEmotionRecommendationFunctions.Keys.ToArray()[_random.Next(0, _negativeEmotionRecommendationFunctions.Keys.Count)];

            return _negativeEmotionRecommendationFunctions[randomRecommendationFunction]();
        }

        private async Task<LogLifeRecommendationResult> GetNegativeEmotionMusicRecommendationAsync()
        {
            var randomMusicTag = MusicTagDictionary.PositiveMusicTagIdDictionary[_random.Next(0, MusicTagDictionary.PositiveMusicTagIdDictionary.Count)];

            var musicRecordTagGrpcDTO = new MusicRecordTagGrpcDTO();
            musicRecordTagGrpcDTO.TagName.Add(randomMusicTag);

            var musicRecord = await _musicRecordGrpcClient.GetRandomMusicRecordByTagAsync(musicRecordTagGrpcDTO);

            return new LogLifeRecommendationResult("NegativeEmotionMusicRecommendationResult", JsonSerializer.Serialize(musicRecord));
        }

        private Task<LogLifeRecommendationResult> GetMixedEmotionRecommendationAsync()
        {
            return Task.FromResult(new LogLifeRecommendationResult("MixedEmotionRecommendationResult", mixedRecommandationReulstContent));
        }
    }
}
