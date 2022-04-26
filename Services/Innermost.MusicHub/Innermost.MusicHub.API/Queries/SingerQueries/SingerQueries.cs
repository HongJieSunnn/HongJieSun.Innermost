using Innermost.MusicHub.API.Queries.SingerQueries.Models;
using Innermost.MusicHub.Domain.AggregatesModels.SingerAggregate;

namespace Innermost.MusicHub.API.Queries.SingerQueries
{
    public class SingerQueries : ISingerQueries
    {
        private readonly MusicHubMongoDBContext _context;
        public SingerQueries(MusicHubMongoDBContext context)
        {
            _context = context;

        }
        public async Task<IEnumerable<SingerDTO>> SearchSingerAsync(string singerName)
        {
            var singerNameTextFilter = Builders<Singer>.Filter.Text(singerName);
            return (await _context.Singers.Find(singerNameTextFilter).ToListAsync()).Select(s=>MapToSingerDTO(s));
        }

        public SingerDTO MapToSingerDTO(Singer singer)
        {
            return new SingerDTO(
                singer.Id!, singer.SingerId, singer.SingerName, singer.SingerAlias,
                singer.SingerNationality, singer.SingerBirthplace,
                singer.SingerOccupation, singer.SingerBirthday, singer.SingerRepresentativeWorks, singer.SingerRegion, singer.SingerCoverUrl,
                singer.SingerAlbums.Select(sa=>new SingerAlbumDTO(sa.Id,sa.AlbumName,sa.AlbumDescriptions,sa.AlbumGenre,sa.AlbumLanguage,sa.AlbumSongCount,sa.PublishCompany,sa.PublishTime)).ToList()
            );
        }
    }
}
