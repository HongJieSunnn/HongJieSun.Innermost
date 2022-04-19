
using Innermost.Meet.API.Queries.SharedLifeRecordQueries;
using Microsoft.AspNetCore.Mvc;


namespace Innermost.Meet.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MeetController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IMeetSharedLifeRecordQueries _meetSharedLifeRecordQueries;
        private readonly IIdentityService _identityService;
        private readonly ILogger<MeetController> _logger;
        public MeetController(IMediator mediator,IMeetSharedLifeRecordQueries meetSharedLifeRecordQueries, IIdentityService identityService, ILogger<MeetController> logger)
        {
            _mediator = mediator;
            _meetSharedLifeRecordQueries = meetSharedLifeRecordQueries;
            _identityService = identityService;
            _logger = logger;

        }

        [HttpPost]
        [Route("meet/like")]
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
    }
}
