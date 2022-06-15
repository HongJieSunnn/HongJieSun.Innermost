using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ReactApp.HttpAggregator.Infrastructure.Enumerations;
using ReactApp.HttpAggregator.Services.Intelligence;
using ReactApp.HttpAggregator.Services.LogLife;

namespace ReactApp.HttpAggregator.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class LogLifeController : ControllerBase
    {
        private readonly ILogLifeApiHttpClient _logLifeApiHttpClient;
        private readonly ILifeRecordIntelligenceService _lifeRecordIntelligenceService;
        public LogLifeController(ILogLifeApiHttpClient logLifeApiHttpClient,ILifeRecordIntelligenceService lifeRecordIntelligenceService)
        {
            _logLifeApiHttpClient=logLifeApiHttpClient;
            _lifeRecordIntelligenceService=lifeRecordIntelligenceService;
        }

        [HttpPost]
        [Route("create")]
        public async Task<IActionResult> CreateLifeRecordAsync([FromBody]LifeRecordModel lifeRecordModel)
        {
            var predictedTag =await _lifeRecordIntelligenceService.GetLifeRecordEmotionTagAsync(lifeRecordModel.Text);

            lifeRecordModel.TagSummaries!.Add(predictedTag.tagId, predictedTag.tagName);

            var createTask = _logLifeApiHttpClient.CreteLifeRecordAsync(lifeRecordModel);
            bool createResult;

            var doRecommendAction = ToDoRecommendAction();
            if (doRecommendAction)
            {
                var recommendTask = _lifeRecordIntelligenceService.GetLifeRecordRecommendationAsync(predictedTag.tagName);

                var recomendationResult = await recommendTask;

                object ans = new object();

                switch (recomendationResult.type)
                {
                    case LifeRecordRecommendations.MixedEmotionRecommendationResult:
                    case LifeRecordRecommendations.PositiveEmotionRecommendationResult:
                        ans = recomendationResult.content;
                        break;
                    case LifeRecordRecommendations.NegativeEmotionMusicRecommendationResult:
                        ans = JsonSerializer.Deserialize<RecommendedMusicRecord>(recomendationResult.content)!;
                        break;
                }

                createResult = await createTask;

                if (!createResult)
                    return BadRequest("创建失败");

                return Ok(new { type = recomendationResult.type, content = ans });
            }

            createResult = await createTask;

            if (!createResult)
                return BadRequest("创建失败");

            return Ok();
        }

        private bool ToDoRecommendAction()
        {
            //var random=new Random();

            //return random.Next(0, 3) == 1;//1/3
            return true;
        }
    }
}
