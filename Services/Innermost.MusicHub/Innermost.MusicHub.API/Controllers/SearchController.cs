﻿using Innermost.MusicHub.API.Queries.AlbumQueries;
using Innermost.MusicHub.API.Queries.AlbumQueries.Models;
using Innermost.MusicHub.API.Queries.MusicRecordQueries;
using Innermost.MusicHub.API.Queries.MusicRecordQueries.Models;
using Innermost.MusicHub.API.Queries.SingerQueries;
using Innermost.MusicHub.API.Queries.SingerQueries.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Innermost.MusicHub.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SearchController : ControllerBase
    {
        private readonly IAlbumQueries _albumQueries;
        private readonly IMusicRecordQueries _musicRecordQueries;
        private readonly ISingerQueries _singerQueries;
        public SearchController(IAlbumQueries albumQueries,IMusicRecordQueries musicRecordQueries,ISingerQueries singerQueries)
        {
            _albumQueries=albumQueries;
            _musicRecordQueries=musicRecordQueries;
            _singerQueries=singerQueries;
        }

        [HttpGet]
        [Route("album")]
        public async Task<ActionResult<IEnumerable<AlbumDTO>>> SearchAlbumAsync(string name)
        {
            var albums=await _albumQueries.SearchAlbum(name);
            return Ok(albums);
        }

        [HttpGet]
        [Route("music")]
        public async Task<ActionResult<IEnumerable<MusicRecordDTO>>> SearchMusicRecordAsync(string name)
        {
            var musicRecords = await _musicRecordQueries.SearchMusicRecordAsync(name);
            return Ok(musicRecords);
        }

        [HttpGet]
        [Route("album")]
        public async Task<ActionResult<IEnumerable<SingerDTO>>> SearchSingerAsync(string name)
        {
            var singers = await _singerQueries.SearchSingerAsync(name);
            return Ok(singers);
        }
    }
}
