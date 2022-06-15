namespace ReactApp.HttpAggregator.Services.LogLife
{
    public interface ILogLifeApiHttpClient
    {
        Task<bool> CreteLifeRecordAsync(LifeRecordModel lifeRecordModel);
    }
}
