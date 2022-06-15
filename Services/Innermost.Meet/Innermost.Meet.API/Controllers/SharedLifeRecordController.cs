using Microsoft.AspNetCore.Mvc;

namespace Innermost.Meet.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class SharedLifeRecordController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IMeetSharedLifeRecordQueries _meetSharedLifeRecordQueries;
        private readonly IUserIdentityService _identityService;
        private readonly ILogger<SharedLifeRecordController> _logger;
        public SharedLifeRecordController(IMediator mediator, IMeetSharedLifeRecordQueries meetSharedLifeRecordQueries, IUserIdentityService identityService, ILogger<SharedLifeRecordController> logger)
        {
            _mediator = mediator;
            _meetSharedLifeRecordQueries = meetSharedLifeRecordQueries;
            _identityService = identityService;
            _logger = logger;

        }

        [HttpPost]
        [Route("like")]
        public async Task<IActionResult> LikeSharedLifeRecordAsync([FromBody] LikeSharedLifeRecordCommand command, [FromHeader(Name = "x-requestid")] string requestId)
        {
            bool commandResult = false;

            if (command.LikerUserId is null)
            {
                command.LikerUserId = _identityService.GetUserId();
            }

            if (Guid.TryParse(requestId, out Guid guid) && guid != Guid.Empty)
            {
                var createCommand = new IdempotentCommandLoader<LikeSharedLifeRecordCommand, bool>(command, guid);

                _logger.LogSendCommand(requestId, nameof(LikeSharedLifeRecordCommand), nameof(command.SharedLifeRecordObjectId), command.SharedLifeRecordObjectId, command);

                commandResult = await _mediator.Send(createCommand);
            }

            if (!commandResult)
                return BadRequest();//TODO 或者直接抛异常 然后 filter 

            return Ok();
        }

        [HttpGet]
        [Route("record/random")]
        public async Task<ActionResult<IEnumerable<SharedLifeRecordDTO>>> GetRandomSharedLifeRecordsAsync([Range(1, 50)] int limit = 20)
        {
            var sharedRecords = await _meetSharedLifeRecordQueries.GetRandomSharedLifeRecordsAsync(limit);

            if (sharedRecords is null)
                return BadRequest();

            return Ok(sharedRecords);
        }

        [HttpGet]
        [Route("record/tag")]
        public async Task<ActionResult<IEnumerable<SharedLifeRecordDTO>>> GetSharedLifeRecordsByTagsAsync(
            IEnumerable<string> tagIds,
            [Range(1, int.MaxValue)] int page = 1,
            [Range(20, 100)] int limit = 20,
            [RegularExpression(@"^Id|CreateTime|LikesCount$")] string sortBy = "Id")
        {
            var sharedRecords = await _meetSharedLifeRecordQueries.GetSharedLifeRecordsByTagsAsync(tagIds, page, limit, sortBy);

            if (sharedRecords is null)
                return BadRequest();

            return Ok(sharedRecords);
        }

        [HttpGet]
        [Route("record/music-record")]
        public async Task<ActionResult<IEnumerable<SharedLifeRecordDTO>>> GetSharedLifeRecordsByMusicRecordAsync(
            string mid,
            [Range(1, int.MaxValue)] int page = 1,
            [Range(20, 100)] int limit = 20,
            [RegularExpression(@"^Id|CreateTime|LikesCount$")] string sortBy = "LikesCount")
        {
            var sharedRecords = await _meetSharedLifeRecordQueries.GetSharedLifeRecordsByMusicRecordAsync(mid, page, limit, sortBy);

            if (sharedRecords is null)
                return BadRequest();

            return Ok(sharedRecords);
        }
    }
}
