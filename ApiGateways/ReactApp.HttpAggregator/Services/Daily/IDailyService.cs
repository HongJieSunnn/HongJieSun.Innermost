using ReactApp.HttpAggregator.Models.Daily;

namespace ReactApp.HttpAggregator.Services.Daily
{
    public interface IDailyService
    {
        Task<string> GetDailySentenceAsync();
        Task<DailyPictureModel?> GetDailyPictureAsync();
    }
}
