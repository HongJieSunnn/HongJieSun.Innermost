using TagS.Microservices.Server.Queries.TagQueries;

namespace Innermost.TagServer.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class TagController : ControllerBase
    {
        private readonly ITagQueries _tagQueries;
        private readonly ILogger<TagController> _logger;
        public TagController(ITagQueries tagQueries,ILogger<TagController> logger)
        {
            _tagQueries = tagQueries;
            _logger = logger;
        }

        [HttpGet]
        [Route("all")]
        public async Task<ActionResult<IEnumerable<Tag>>> GetAllTagsAsync()
        {
            var tags=await _tagQueries.GetAllTagsAsync();
            return Ok(tags);
        }

        [HttpGet]
        [Route("first")]
        public async Task<ActionResult<IEnumerable<Tag>>> GetFirstLevelTagAsync()
        {
            var tags=await _tagQueries.GetAllFirstLevelTagsAsync();
            return Ok(tags);
        }

        [HttpGet]
        [Route("next/{tagId}")]
        public async Task<ActionResult<IEnumerable<Tag>>> GetNextTagsAsync(string tagId)
        {
            var tags = await _tagQueries.GetNextTagsAsync(tagId);
            return Ok(tags);
        }

        [HttpGet]
        [Route("{name}")]
        public async Task<ActionResult<Tag>> GetTagByNameAsync(string name)
        {
            var tags = await _tagQueries.GetTagByPreferredNameAsync(name);
            return Ok(tags);
        }

        [HttpGet]
        [Route("{synonym}")]
        public async Task<ActionResult<Tag>> GetTagBySynonymAsync(string synonym)
        {
            var tags = await _tagQueries.GetTagBySynonymAsync(synonym);
            return Ok(tags);
        }
    }
}
