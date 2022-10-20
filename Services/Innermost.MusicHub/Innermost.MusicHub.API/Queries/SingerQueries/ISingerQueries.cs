using Innermost.MusicHub.API.Queries.SingerQueries.Models;

namespace Innermost.MusicHub.API.Queries.SingerQueries
{
    public interface ISingerQueries
    {
        Task<IEnumerable<SingerDTO>> SearchSingerAsync(string singerName, int page = 1, int limit = 10);
    }
}
