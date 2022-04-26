using Innermost.MusicHub.API.Queries.MusicRecordQueries.Models;
using Innermost.MusicHub.Domain.AggregatesModels.MusicRecordAggregate;
using System.Linq.Expressions;

namespace Innermost.MusicHub.API.Queries.MusicRecordQueries
{
    public interface IMusicRecordQueries
    {
        Task<MusicRecordDTO> GetOneRandomMusicRecord(Expression<Func<MusicRecord, bool>>? filterDefinition =null);
        Task<IEnumerable<MusicRecordDTO>> SearchMusicRecordAsync(string musicRecordName);
    }
}
