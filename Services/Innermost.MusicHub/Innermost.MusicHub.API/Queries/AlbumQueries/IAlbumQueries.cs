using Innermost.MusicHub.API.Queries.AlbumQueries.Models;

namespace Innermost.MusicHub.API.Queries.AlbumQueries
{
    public interface IAlbumQueries
    {
        Task<IEnumerable<AlbumDTO>> SearchAlbum(string albumName,int page = 1, int limit=10);
    }
}
