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
        public async Task<IEnumerable<AlbumDTO>> SearchAlbum(string albumName)
        {
            var textFilter = Builders<Album>.Filter.Text(albumName);

            return (await _context.Albums.Find(textFilter).ToListAsync()).Select(a => MapToAlbumDTO(a));
        }

        private AlbumDTO MapToAlbumDTO(Album album)
        {
            return new AlbumDTO(
                album.Id!,
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
                        mr.Id!,
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
