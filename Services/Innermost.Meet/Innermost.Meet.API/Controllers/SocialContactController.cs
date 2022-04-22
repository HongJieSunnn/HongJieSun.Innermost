using Innermost.Meet.API.Queries.SharedLifeRecordQueries;
using Microsoft.AspNetCore.Mvc;


namespace Innermost.Meet.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SocialContactController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IMeetSharedLifeRecordQueries _meetSharedLifeRecordQueries;
        private readonly IIdentityService _identityService;
        private readonly ILogger<SocialContactController> _logger;

        private readonly static string SuccessedString = string.Empty;
        public SocialContactController(IMediator mediator, IMeetSharedLifeRecordQueries meetSharedLifeRecordQueries, IIdentityService identityService, ILogger<SocialContactController> logger)
        {
            _mediator = mediator;
            _meetSharedLifeRecordQueries = meetSharedLifeRecordQueries;
            _identityService = identityService;
            _logger = logger;

        }

        [HttpPost]
        [Route("confidant-request")]
        public async Task<IActionResult> AddConfidantRequestAsync([FromBody] AddConfidantRequestCommand command, [FromHeader(Name = "x-requestid")] string requestId)
        {
            string commandResult = SuccessedString;

            if (command.RequestUserId is null)
            {
                command.RequestUserId = _identityService.GetUserId();
            }

            if (Guid.TryParse(requestId, out Guid guid) && guid != Guid.Empty)
            {
                var addConfidantRequestCommand = new IdempotentCommandLoader<AddConfidantRequestCommand, string>(command, guid);

                _logger.LogSendCommand(requestId, nameof(LikeSharedLifeRecordCommand), $"{nameof(command.RequestUserId)}->{nameof(command.ToUserId)}", $"{command.RequestUserId}->{command.ToUserId}", command);

                commandResult = await _mediator.Send(addConfidantRequestCommand);
            }

            if (commandResult != SuccessedString)
                return BadRequest(commandResult);

            return Ok();
        }

        [HttpPost]
        [Route("set-confidant-request-statue")]
        public async Task<IActionResult> SetConfidantRequestStatueAsync([FromBody] SetConfidantRequestStatueCommand command, [FromHeader(Name = "x-requestid")] string requestId)
        {
            string commandResult = SuccessedString;

            if (command.UserId is null)
            {
                command.UserId = _identityService.GetUserId();
            }

            if (Guid.TryParse(requestId, out Guid guid) && guid != Guid.Empty)
            {
                var setConfidantRequestStatueCommand = new IdempotentCommandLoader<SetConfidantRequestStatueCommand, string>(command, guid);

                _logger.LogSendCommand(requestId, nameof(LikeSharedLifeRecordCommand), nameof(command.UserId), command.UserId, command);

                commandResult = await _mediator.Send(setConfidantRequestStatueCommand);
            }

            if (commandResult != SuccessedString)
                return BadRequest(commandResult);

            return Ok();
        }
    }
}
