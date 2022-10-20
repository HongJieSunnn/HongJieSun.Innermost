using Innermost.MusicHub.API.Queries.MusicRecordQueries.Models;
using Innermost.MusicHub.Domain.AggregatesModels.MusicRecordAggregate;

namespace Innermost.MusicHub.API.Queries.MusicRecordQueries
{
    public interface IMusicRecordQueries
    {
        Task<MusicRecordDTO> GetOneRandomMusicRecord(FilterDefinition<MusicRecord>? filter = null);
        Task<IEnumerable<MusicRecordDTO>> SearchMusicRecordAsync(string musicRecordName, int page = 1, int limit = 25);
    }
}
