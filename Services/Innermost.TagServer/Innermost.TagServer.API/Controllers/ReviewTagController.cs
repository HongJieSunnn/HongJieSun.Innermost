using Innermost.TagReferrers;
using TagS.Microservices.Server.Queries.TagReviewedQueries;
using TagS.Microservices.Server.Repositories.TagWithReferrerRepository;

namespace Innermost.TagServer.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ReviewTagController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IIdentityService _identityService;
        private readonly ITagReviewedQueries _tagReviewedQueries;
        private readonly ILogger<ReviewTagController> _logger;
        public ReviewTagController(IMediator mediator,IIdentityService identityService,ITagReviewedQueries tagReviewedQueries,ILogger<ReviewTagController> logger)
        {
            _mediator=mediator;
            _identityService=identityService;
            _tagReviewedQueries=tagReviewedQueries;
            _logger=logger;
        }

        [HttpPost]
        [Route("create")]
        public async Task<IActionResult> CreateReviewedTagAsync([FromBody] CreateReviewedTagCommand command, [FromHeader(Name = "x-requestid")] string requestId)
        {
            var commandSuccess = false;

            if(command.UserId is null)
            {
                command.UserId = _identityService.GetUserId();
            }

            if (Guid.TryParse(requestId, out Guid guid) && guid != Guid.Empty)
            {
                var idempotentCommand = new IdempotentCommandLoader<CreateReviewedTagCommand, bool>(command, guid);

                _logger.LogInformation("Start creating reviewed tag ({@Command})", command);

                commandSuccess = await _mediator.Send(idempotentCommand);
            }

            if (!commandSuccess)
                return BadRequest($"Tag with PreferredName{command.PreferredTagName} is already existed.");
            return Ok();
        }

        [HttpPost]
        [Route("pass")]
        //[Authorize(Policy ="Admin")]//TODO uncomment
        public async Task<IActionResult> PassReviewedTagAsync([FromBody] PassReviewedTagCommand command, [FromHeader(Name = "x-requestid")] string requestId)
        {
            var commandSuccess = false;

            if (Guid.TryParse(requestId, out Guid guid) && guid != Guid.Empty)
            {
                var idempotentCommand = new IdempotentCommandLoader<PassReviewedTagCommand, bool>(command, guid);

                _logger.LogInformation("Start passing reviewed tag ({@Command})", command);

                commandSuccess = await _mediator.Send(idempotentCommand);
            }

            if (!commandSuccess)
                return BadRequest();
            return Ok();
        }

        [HttpPost]
        [Route("refuse")]
        //[Authorize(Policy = "Admin")]
        public async Task<IActionResult> RefuseReviewedTagAsync([FromBody] RefuseReviewedTagCommand command, [FromHeader(Name = "x-requestid")] string requestId)
        {
            var commandSuccess = false;

            if (Guid.TryParse(requestId, out Guid guid) && guid != Guid.Empty)
            {
                var idempotentCommand = new IdempotentCommandLoader<RefuseReviewedTagCommand, bool>(command, guid);

                _logger.LogInformation("Start refusing reviewed tag ({@Command})", command);

                commandSuccess = await _mediator.Send(idempotentCommand);
            }

            if (!commandSuccess)
                return BadRequest();
            return Ok();
        }

        [HttpGet]
        [Route("all")]
        public async Task<ActionResult<IEnumerable<TagReviewedDTO>>> GetAllToBeReviewedTagsAsync()
        {
            var tags=await _tagReviewedQueries.GetTobeReviewedTagsAsync();
            return Ok(tags);
        }
    }
}
