namespace Innermost.LogLife.Domain.AggregatesModels.LifeRecordAggregate
{
    public interface ILifeRecordRepository
        : IRepository<LifeRecord>
    {
        Task<LifeRecord> AddAsync(LifeRecord lifeRecord);
        LifeRecord Update(LifeRecord lifeRecord);
        Task<LifeRecord?> DeleteAsync(int id,string userId);
        Task<LifeRecord?> GetRecordByIdAsync(int id, string userId);
    }
}
