using Innermost.Search.API.Models.BaiduMapModels;

namespace Innermost.Search.API.Services.BaiduMapServices
{
    public interface IBaiduMapService
    {
        Task<BaiduMapLocatedByIPResponseDTO> GetLocationByIPAsync();
    }
}
