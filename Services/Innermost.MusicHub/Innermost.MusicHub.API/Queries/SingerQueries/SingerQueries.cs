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
        public async Task<IEnumerable<SingerDTO>> SearchSingerAsync(string singerName, int page = 1, int limit = 10)
        {
            var textFilter = Builders<Singer>.Filter.Regex(mr => mr.SingerName, new BsonRegularExpression($@"{singerName}", "i"));

            return (await _context.Singers.Find(textFilter).Skip((page - 1) * limit).Limit(limit).ToListAsync()).Select(m => MapToSingerDTO(m));
        }

        public SingerDTO MapToSingerDTO(Singer singer)
        {
            return new SingerDTO(
                singer.SingerMid!, singer.SingerId, singer.SingerName, singer.SingerAlias,
                singer.SingerNationality, singer.SingerBirthplace,
                singer.SingerOccupation, singer.SingerBirthday, singer.SingerRepresentativeWorks, singer.SingerRegion, singer.SingerCoverUrl,
                singer.SingerAlbums.Select(sa => new SingerAlbumDTO(sa.AlbumMid, sa.AlbumName, sa.AlbumDescriptions, sa.AlbumGenre, sa.AlbumLanguage, sa.AlbumCoverUrl, sa.AlbumSongCount, sa.PublishCompany, sa.PublishTime)).ToList()
            );
        }
    }
}
