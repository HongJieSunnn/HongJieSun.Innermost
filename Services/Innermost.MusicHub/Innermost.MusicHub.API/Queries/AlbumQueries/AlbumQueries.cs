using Innermost.MusicHub.API.Queries.AlbumQueries.Models;
using Innermost.MusicHub.Domain.AggregatesModels.AlbumAggregate;

namespace Innermost.MusicHub.API.Queries.AlbumQueries
{
    public class AlbumQueries : IAlbumQueries
    {
        private readonly MusicHubMongoDBContext _context;
        public AlbumQueries(MusicHubMongoDBContext context)
        {
            _context=context;
        }
        public async Task<IEnumerable<AlbumDTO>> SearchAlbum(string albumName, int page = 1, int limit = 10)
        {
            var textFilter = Builders<Album>.Filter.Regex(mr => mr.AlbumName, new BsonRegularExpression($"^{albumName}","i"));

            return (await _context.Albums.Find(textFilter).Skip((page - 1) * limit).Limit(limit).ToListAsync()).OrderBy(a=>a.PublishTime).Select(m => MapToAlbumDTO(m));
        }

        private AlbumDTO MapToAlbumDTO(Album album)
        {
            return new AlbumDTO(
                album.AlbumMid,
                album.AlbumId,
                album.AlbumName,
                album.AlbumDescriptions,
                album.AlbumGenre,
                album.AlbumLanguage,
                album.AlbumCoverUrl,
                album.AlbumSingerName,
                album.AlbumSingerMid,
                album.AlbumSongCount,
                album.PublishCompany,
                album.PublishTime,
                album.MusicRecords.Select(mr=>
                    new AlbumMusicRecordDTO(
                        mr.MusicMid,
                        mr.MusicName,
                        mr.TranslatedMusicName??"",
                        mr.Genre,
                        mr.Language,
                        mr.MusicUrl,
                        mr.AlbumSingers.Select(s=>new AlbumSingerDTO(s.Id,s.SingerName)).ToList()
                    )
                ).ToList()
            );
        }
    }
}
