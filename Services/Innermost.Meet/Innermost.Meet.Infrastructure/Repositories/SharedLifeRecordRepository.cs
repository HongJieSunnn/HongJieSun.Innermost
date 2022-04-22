namespace Innermost.Meet.Infrastructure.Repositories
{
    internal class SharedLifeRecordRepository : ISharedLifeRecordRepository
    {
        private readonly MeetMongoDBContext _context;
        private readonly IClientSessionHandle _session;
        public SharedLifeRecordRepository(MeetMongoDBContext context, IClientSessionHandle session)
        {
            _context = context;
            _session = session;

        }
        public IUnitOfWork UnitOfWork => _context;

        public Task<SharedLifeRecord> GetSharedLifeRecordAsync(string sharedLifeRecordObjectId)
        {
            return _context.SharedLifeRecords.Find(l=>l.Id==sharedLifeRecordObjectId).FirstAsync();
        }

        public Task AddSharedLifeRecordAsync(SharedLifeRecord sharedLifeRecord)
        {
            return _context.SharedLifeRecords.InsertOneAsync(_session, sharedLifeRecord);
        }

        public Task<UpdateResult> UpdateSharedLifeRecordAsync(string sharedLifeRecordObjectId, UpdateDefinition<SharedLifeRecord> updateDefinition, params FilterDefinition<SharedLifeRecord>[] filterDefinitions)
        {
            var filter=CombineFilterDefinitions(Builders<SharedLifeRecord>.Filter.Eq(l => l.Id, sharedLifeRecordObjectId), filterDefinitions);

            return _context.SharedLifeRecords.UpdateOneAsync(_session, filter , updateDefinition);
        }

        public Task<UpdateResult> UpdateSharedLifeRecordAsync(int lifeRecordId, UpdateDefinition<SharedLifeRecord> updateDefinition, params FilterDefinition<SharedLifeRecord>[] filterDefinitions)
        {
            var filter = CombineFilterDefinitions(Builders<SharedLifeRecord>.Filter.Eq(l => l.RecordId, lifeRecordId), filterDefinitions);

            return _context.SharedLifeRecords.UpdateOneAsync(_session, filter, updateDefinition);
        }

        private FilterDefinition<SharedLifeRecord> CombineFilterDefinitions(FilterDefinition<SharedLifeRecord> firstFilterDefinition, IEnumerable<FilterDefinition<SharedLifeRecord>> filterDefinitions)
        {
            var filter = firstFilterDefinition;
            foreach (var filterDefinition in filterDefinitions)
            {
                filter &= filterDefinition;
            }
            return filter;
        }

        public Task<ReplaceOneResult> UpdateSharedLifeRecordAsync(SharedLifeRecord sharedLifeRecord)
        {
            return _context.SharedLifeRecords.ReplaceOneAsync(_session, l => l.Id == sharedLifeRecord.Id, sharedLifeRecord);
        }

        public Task<UpdateResult> UpdateManySharedLifeRecordsAsync(IEnumerable<int> lifeRecordIds, UpdateDefinition<SharedLifeRecord> updateDefinition, params FilterDefinition<SharedLifeRecord>[] filterDefinitions)
        {
            var idFilter= Builders<SharedLifeRecord>.Filter.In(l=>l.RecordId, lifeRecordIds);
            var filter = CombineFilterDefinitions(idFilter, filterDefinitions);

            return _context.SharedLifeRecords.UpdateOneAsync(_session, filter, updateDefinition);
        }

        public Task<UpdateResult> UpdateManySharedLifeRecordsAsync(IEnumerable<string> sharedLifeRecordObjectIds, UpdateDefinition<SharedLifeRecord> updateDefinition, params FilterDefinition<SharedLifeRecord>[] filterDefinitions)
        {
            var idFilter = Builders<SharedLifeRecord>.Filter.In(l => l.Id, sharedLifeRecordObjectIds);
            var filter = CombineFilterDefinitions(idFilter, filterDefinitions);

            return _context.SharedLifeRecords.UpdateOneAsync(_session, filter, updateDefinition);
        }

        public async Task<UpdateResult> DeleteSharedLifeRecordAsync(int lifeRecordId)
        {
            var filter = Builders<SharedLifeRecord>.Filter.Eq(l => l.RecordId, lifeRecordId);
            var record = await _context.SharedLifeRecords.Find(filter).FirstAsync();
            var update = record.SetDeleted();
            return await _context.SharedLifeRecords.UpdateOneAsync(_session, filter, update);
        }

        public async Task<UpdateResult> DeleteSharedLifeRecordAsync(string sharedLifeRecordObjectId)
        {
            var filter = Builders<SharedLifeRecord>.Filter.Eq(l => l.Id, sharedLifeRecordObjectId);
            var record = await _context.SharedLifeRecords.Find(filter).FirstAsync();
            var update = record.SetDeleted();
            return await _context.SharedLifeRecords.UpdateOneAsync(_session, filter, update);
        }

        public Task<UpdateResult> DeleteSharedLifeRecordAsync(SharedLifeRecord sharedLifeRecord)
        {
            return _context.SharedLifeRecords.UpdateOneAsync(_session, l => l.Id == sharedLifeRecord.Id, sharedLifeRecord.SetDeleted());
        }
    }
}
