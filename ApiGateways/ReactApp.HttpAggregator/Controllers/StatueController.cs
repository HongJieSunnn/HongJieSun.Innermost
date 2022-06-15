using Innermost.Identity.API.UserStatue;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ReactApp.Aggregator.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class StatueController : ControllerBase
    {
        private readonly IIdentityService _identityService;
        private readonly IdentityUserStatueGrpc.IdentityUserStatueGrpcClient _identityUserStatueGrpcClient;
        public StatueController(IIdentityService identityService, IdentityUserStatueGrpc.IdentityUserStatueGrpcClient identityUserStatueGrpcClient)
        {
            _identityService = identityService;
            _identityUserStatueGrpcClient = identityUserStatueGrpcClient;

        }

        [HttpPut]
        [Route("change-userstatue")]
        public async Task<IActionResult> ChangeUserStatueAsync(
            [RegularExpression(@"^NORMAL|HAPPY|SAD|ANGRY|DEPRESSION|BORING|LAUGH|BAD|SPEECHLESS|FEAR|LONELY|RELEXED$")]
            string userStatue
        )
        {
            var userId = _identityService.GetUserId();

            var result = await _identityUserStatueGrpcClient.SetUserStatueAsync(new SetUserStatueGrpcDTO() { UserId = userId, UserStatue = userStatue });

            return Ok();
        }
    }
}
