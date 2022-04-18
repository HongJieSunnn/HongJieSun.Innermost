using Innermost.Meet.API.Queries.SharedLifeRecordQueries;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace Innermost.Meet.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MeetController : ControllerBase
    {
        private readonly IMeetSharedLifeRecordQueries _meetSharedLifeRecordQueries;
        public MeetController(IMeetSharedLifeRecordQueries meetSharedLifeRecordQueries)
        {
            _meetSharedLifeRecordQueries=meetSharedLifeRecordQueries;
        }

        [HttpGet]
        [Route("record/tag")]
        public async Task<ActionResult<IEnumerable<SharedLifeRecordDTO>>> GetSharedLifeRecordsByTagsAsync(
            IEnumerable<string> tagIds,
            [Range(1,int.MaxValue)] int page = 1, 
            [Range(20,100)]int limit = 20, 
            [RegularExpression(@"^Id|CreateTime|LikesCount$")]string sortBy = "Id")
        {
            var sharedRecords = await _meetSharedLifeRecordQueries.GetSharedLifeRecordsByTagsAsync(tagIds, page, limit, sortBy);

            if (sharedRecords is null)
                return BadRequest();

            return Ok(sharedRecords);
        }
    }
}
