namespace Innermost.LogLife.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class LogLifeController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IIdentityService _identityService;
        private readonly ILifeRecordQueries _lifeRecordQueries;
        private readonly ILogger<LogLifeController> _logger;
        public LogLifeController(IMediator mediator, IIdentityService identityService, ILifeRecordQueries lifeRecordQueries, ILogger<LogLifeController> logger)
        {
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            _identityService = identityService ?? throw new ArgumentNullException(nameof(identityService));
            _lifeRecordQueries = lifeRecordQueries ?? throw new ArgumentNullException(nameof(lifeRecordQueries));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        /// <summary>
        /// Api to create one record
        /// </summary>
        /// <param name="record"></param>
        /// <param name="requestId">from getway</param>
        /// <returns></returns>
        [Route("create")]
        [HttpPost]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> CreateRecordAsync([FromBody] CreateRecordCommand record, [FromHeader(Name = "x-requestid")] string requestId)
        {
            bool commandResult = false;

            if(record.UserId is null)
            {
                record.UserId =_identityService.GetUserId();
            }

            if (Guid.TryParse(requestId, out Guid guid) && guid != Guid.Empty)
            {
                var createCommand = new IdempotentCommandLoader<CreateRecordCommand, bool>(record, guid);

                _logger.LogInformation("");//TODO

                commandResult = await _mediator.Send(createCommand);
            }

            if (!commandResult)
                return BadRequest();//TODO 或者直接抛异常 然后 filter 

            return Ok();
        }

        [HttpDelete]
        [Route("delete/{recordId:int}")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> DeleteOneRecordAsync([FromBody] DeleteRecordCommand command, [FromHeader(Name = "x-requestid")] string requestId)//TODO Add Attribute to ensure the recordId belongs to user who requests
        {
            var commandResult = false;

            if(command.UserId is null)
            {
                command.UserId =_identityService.GetUserId();
            }

            if (Guid.TryParse(requestId, out Guid guid) && guid != Guid.Empty)
            {
                var deleteCommand = new IdempotentCommandLoader<DeleteRecordCommand, bool>(command, guid);

                _logger.LogInformation("");//TODO

                commandResult = await _mediator.Send(deleteCommand);
            }

            if (!commandResult)
                return BadRequest($"Record with Id {command.RecordId} is not existed or has been deleted.");

            return Ok();
        }

        [HttpPut]
        [Route("set-shared")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> SetRecordSharedAsync([FromBody] SetRecordSharedCommand command, [FromHeader(Name = "x-requestid")] string requestId)
        {
            var commandResult = false;

            if(command.UserId ==null)
            {
                command.UserId=_identityService.GetUserId();
            }

            if (Guid.TryParse(requestId, out Guid guid) && guid != Guid.Empty)
            {
                var setSharedCommand = new IdempotentCommandLoader<SetRecordSharedCommand, bool>(command, guid);

                _logger.LogInformation("");//TODO

                commandResult = await _mediator.Send(setSharedCommand);
            }

            if (!commandResult)
                return BadRequest($"Record with Id {command.RecordId} is not existed or does not belong to the requested user.");

            return Ok();
        }

        [HttpGet]
        [Route("records")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        public async Task<ActionResult<IEnumerable<LifeRecordDTO>>> GetAllRecordsAsync()
        { 
            var records =await _lifeRecordQueries.GetAllRecordsAsync()??new List<LifeRecordDTO>();
            return Ok(records.Reverse());
        }

        [HttpGet]
        [Route("record/{id:int}")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<ActionResult<LifeRecordDTO>> GetRecordByIdAsync(int id)
        {
            var records = await _lifeRecordQueries.FindRecordByRecordId(id);
            if(records is null)
                return BadRequest($"The record with id:{id} is not existed.");
            return Ok(records);
        }

        [HttpGet]
        [Route("records/tag")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<ActionResult<IEnumerable<LifeRecordDTO>>> GetRecordByTagIdAsync(string tagId)
        {
            var records = await _lifeRecordQueries.FindRecordsByTagIdAsync(tagId)??new List<LifeRecordDTO>();
            return Ok(records);
        }

        [HttpGet]
        [Route("records/datetime")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<ActionResult<IEnumerable<LifeRecordDTO>>> GetRecordByDateTimeAsync(string year,string? month,string? day,string findType)
        {
            var dateTimeToFind=new DateTimeToFind(year,month,day,findType);
            Validator.ValidateObject(dateTimeToFind,new ValidationContext(dateTimeToFind));
            var records = await _lifeRecordQueries.FindRecordsByCreateTimeAsync(dateTimeToFind) ?? new List<LifeRecordDTO>();
            return Ok(records);
        }
    }
}
