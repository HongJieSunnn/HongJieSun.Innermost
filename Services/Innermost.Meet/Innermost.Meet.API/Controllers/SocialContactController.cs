using Microsoft.AspNetCore.Mvc;


namespace Innermost.Meet.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class SocialContactController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ISocialContactQueries _socialContactQueries;
        private readonly IUserIdentityService _identityService;
        private readonly ILogger<SocialContactController> _logger;

        private readonly static string SuccessedString = string.Empty;
        public SocialContactController(IMediator mediator, ISocialContactQueries socialContactQueries, IUserIdentityService identityService, ILogger<SocialContactController> logger)
        {
            _mediator = mediator;
            _socialContactQueries = socialContactQueries;
            _identityService = identityService;
            _logger = logger;

        }

        [HttpPost]
        [Route("add-confidant-request")]
        public async Task<IActionResult> AddConfidantRequestAsync([FromBody] AddConfidantRequestCommand command, [FromHeader(Name = "x-requestid")] string requestId)
        {
            string commandResult = SuccessedString;

            if (command.RequestUserId is null)//TODO for testing.While testing end,userId should only be get by identityService.
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
                return Ok(commandResult);

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

        [HttpGet]
        [Route("confidants")]
        public async Task<ActionResult<IEnumerable<ConfidantRequestDTO>>> GetConfidantsAsync()
        {
            var confidants = await _socialContactQueries.GetConfidantsAsync();
            return Ok(confidants);
        }

        [HttpGet]
        [Route("confidant-requests-to-be-reviewed")]
        public async Task<ActionResult<IEnumerable<ConfidantRequestDTO>>> GetConfidantRequestsToBeReviewedAsync()
        {
            var confidantRequests = await _socialContactQueries.GetConfidantRequestsToBeReviewedAsync();
            return Ok(confidantRequests);
        }
    }
}
