using Innermost.Meet.Domain.AggregatesModels.SharedLifeRecord;

namespace Innermost.Meet.Domain.Repositories
{
    public interface ISharedLifeRecordRepository : IRepository<SharedLifeRecord>
    {
        Task AddSharedLifeRecordAsync(SharedLifeRecord sharedLifeRecord);

        Task<UpdateResult> UpdateSharedLifeRecordAsync(string sharedLifeRecordObjectId,UpdateDefinition<SharedLifeRecord> updateDefinition);
        Task<UpdateResult> UpdateSharedLifeRecordAsync(int lifeRecordId, UpdateDefinition<SharedLifeRecord> updateDefinition);
        Task<ReplaceOneResult> UpdateSharedLifeRecordAsync(SharedLifeRecord sharedLifeRecord);

        Task<UpdateResult> DeleteSharedLifeRecordAsync(int lifeRecordId);
        Task<UpdateResult> DeleteSharedLifeRecordAsync(string sharedLifeRecordObjectId);
        Task<UpdateResult> DeleteSharedLifeRecordAsync(SharedLifeRecord sharedLifeRecord);
    }
}
