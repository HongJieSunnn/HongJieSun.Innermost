using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Innermost.Meet.SignalRHub.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ChattingContextController : ControllerBase
    {
        private readonly IUserChattingContextQueries _userChattingContextQueries;
        private readonly IIdentityService _identityService;
        public ChattingContextController(IUserChattingContextQueries userChattingContextQueries, IIdentityService identityService)
        {
            _userChattingContextQueries = userChattingContextQueries;
            _identityService = identityService;
        }

        [HttpGet]
        [Route("chatting-records")]
        public async Task<ActionResult<ChattingRecordDTO>> GetChattingRecordsAsync(string chattingContextId, int page = 1, int limit = 50)
        {
            var userId = _identityService.GetUserId();
            var chattingContextIds = await _userChattingContextQueries.GetAllChattingContextIdsOfUserAsync(userId);
            if (!chattingContextIds.Contains(chattingContextId))
                return Unauthorized($"ChattingContext(id:{chattingContextId}) does not belong to requested user(id:{userId})");

            var chattingRecords = await _userChattingContextQueries.GetChattingRecordsAsync(chattingContextId, page, limit);
            return Ok(chattingRecords);
        }
    }
}
