using Innermost.MusicHub.API.Queries.AlbumQueries.Models;

namespace Innermost.MusicHub.API.Queries.AlbumQueries
{
    public interface IAlbumQueries
    {
        Task<IEnumerable<AlbumDTO>> SearchAlbum(string albumName);
    }
}
