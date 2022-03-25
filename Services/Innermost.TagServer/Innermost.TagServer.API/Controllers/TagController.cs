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
        public async Task<ActionResult<IEnumerable<TagDTO>>> GetAllTagsAsync()//TODO can select from TagReferrer
        {
            var tags=await _tagQueries.GetAllTagsAsync();
            return Ok(tags);
        }

        [HttpGet]
        [Route("first")]
        public async Task<ActionResult<IEnumerable<TagDTO>>> GetFirstLevelTagAsync()//TODO FirstLevelTagDTO
        {
            var tags=await _tagQueries.GetAllFirstLevelTagsAsync();
            return Ok(tags);
        }

        [HttpGet]
        [Route("next")]
        public async Task<ActionResult<IEnumerable<TagDTO>>> GetNextTagsAsync(string tagId)//TODO fisrtLevelId and previousTagId
        {
            var tags = await _tagQueries.GetNextTagsAsync(tagId);
            return Ok(tags);
        }

        [HttpGet]
        [Route("name")]
        public async Task<ActionResult<TagDTO>> GetTagByNameAsync(string name)
        {
            var tags = await _tagQueries.GetTagByPreferredNameAsync(name);
            return Ok(tags);
        }

        [HttpGet]
        [Route("synonym")]
        public async Task<ActionResult<TagDTO>> GetTagBySynonymAsync(string synonym)
        {
            var tags = await _tagQueries.GetTagBySynonymAsync(synonym);//TODO now will call The expression tree is not supported: {document}.Synonyms. List Synonyms is ok but IReadOnlyCollection Synonyms is not ok.
            return Ok(tags);
        }
    }
}
