using Innermost.Meet.Domain.AggregatesModels.SharedLifeRecordAggregate;

namespace Innermost.Meet.Domain.Repositories
{
    public interface ISharedLifeRecordRepository : IRepository<SharedLifeRecord>
    {
        Task<SharedLifeRecord> GetSharedLifeRecordAsync(string sharedLifeRecordObjectId);

        Task AddSharedLifeRecordAsync(SharedLifeRecord sharedLifeRecord);

        /// <summary>
        /// UpdateSharedLifeRecordAsync
        /// </summary>
        /// <typeparam name="TElement">Element type to update which may be aggregateroot or element.</typeparam>
        /// <param name="sharedLifeRecordObjectId"></param>
        /// <param name="updateDefinition"></param>
        /// <param name="filterDefinitions">Sometimes we may update elements,so we need more filterDefinitions.</param>
        /// <returns></returns>
        Task<UpdateResult> UpdateSharedLifeRecordAsync(string sharedLifeRecordObjectId, UpdateDefinition<SharedLifeRecord> updateDefinition, params FilterDefinition<SharedLifeRecord>[] filterDefinitions);
        Task<UpdateResult> UpdateSharedLifeRecordAsync(int lifeRecordId, UpdateDefinition<SharedLifeRecord> updateDefinition,params FilterDefinition<SharedLifeRecord>[] filterDefinitions);
        Task<ReplaceOneResult> UpdateSharedLifeRecordAsync(SharedLifeRecord sharedLifeRecord);


        Task<UpdateResult> UpdateManySharedLifeRecordsAsync(IEnumerable<int> lifeRecordIds, UpdateDefinition<SharedLifeRecord> updateDefinition, params FilterDefinition<SharedLifeRecord>[] filterDefinitions);
        Task<UpdateResult> UpdateManySharedLifeRecordsAsync(IEnumerable<string> sharedLifeRecordObjectIds, UpdateDefinition<SharedLifeRecord> updateDefinition, params FilterDefinition<SharedLifeRecord>[] filterDefinitions);

        Task<UpdateResult> DeleteSharedLifeRecordAsync(int lifeRecordId);
        Task<UpdateResult> DeleteSharedLifeRecordAsync(string sharedLifeRecordObjectId);
        Task<UpdateResult> DeleteSharedLifeRecordAsync(SharedLifeRecord sharedLifeRecord);
    }
}
