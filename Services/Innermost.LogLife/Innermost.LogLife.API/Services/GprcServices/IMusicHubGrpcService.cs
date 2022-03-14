namespace Innemost.LogLife.API.Services.GprcServices
{
    public interface IMusicHubGrpcService
    {
        Task<MusicDetail> GetMusicDetailByMusicId(int id);
    }
}
