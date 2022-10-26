using Microsoft.AspNetCore.Mvc;
using ReactApp.HttpAggregator.Services.Daily;

namespace ReactApp.HttpAggregator.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DailyController : ControllerBase
    {
        private readonly IDailyService _dailyService;
        public DailyController(IDailyService dailyService)
        {
            _dailyService = dailyService;
        }

        [HttpGet]
        [Route("sentence")]
        public async Task<IActionResult> GetDailySentenceAsync()
        {
            var dailySentence = await _dailyService.GetDailySentenceAsync();

            return Ok(dailySentence);
        }

        [HttpGet]
        [Route("picture")]
        public async Task<IActionResult> GetDailyPictureAsync()
        {
            var dailyPicture = await _dailyService.GetDailyPictureAsync();

            if (dailyPicture == null)
                return Ok("无");

            return Ok(dailyPicture);
        }
    }
}
