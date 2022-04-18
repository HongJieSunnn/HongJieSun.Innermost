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

        public Task AddSharedLifeRecordAsync(SharedLifeRecord sharedLifeRecord)
        {
            return _context.SharedLifeRecords.InsertOneAsync(_session,sharedLifeRecord);
        }

        public Task<UpdateResult> UpdateSharedLifeRecordAsync(string sharedLifeRecordObjectId, UpdateDefinition<SharedLifeRecord> updateDefinition)
        {
            return _context.SharedLifeRecords.UpdateOneAsync(_session, l =>l.Id==sharedLifeRecordObjectId, updateDefinition);
        }

        public Task<UpdateResult> UpdateSharedLifeRecordAsync(int lifeRecordId, UpdateDefinition<SharedLifeRecord> updateDefinition)
        {
            return _context.SharedLifeRecords.UpdateOneAsync(_session, l => l.RecordId == lifeRecordId, updateDefinition);
        }

        public Task<ReplaceOneResult> UpdateSharedLifeRecordAsync(SharedLifeRecord sharedLifeRecord)
        {
            return _context.SharedLifeRecords.ReplaceOneAsync(_session, l =>l.Id==sharedLifeRecord.Id,sharedLifeRecord);
        }

        public async Task<UpdateResult> DeleteSharedLifeRecordAsync(int lifeRecordId)
        {
            var filter=Builders<SharedLifeRecord>.Filter.Eq(l=>l.RecordId,lifeRecordId);
            var record= await _context.SharedLifeRecords.Find(filter).FirstAsync();
            var update= record.SetDeleted();
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
            return _context.SharedLifeRecords.UpdateOneAsync(_session, l =>l.Id==sharedLifeRecord.Id,sharedLifeRecord.SetDeleted());
        }
    }
}
