namespace Innermost.LogLife.Infrastructure.Repositories
{
    public class LifeRecordRepository
        : ILifeRecordRepository
    {
        private readonly LifeRecordDbContext _context;
        public IUnitOfWork UnitOfWork => _context;
        public LifeRecordRepository(LifeRecordDbContext context)
        {
            _context = context;
        }

        public async Task<LifeRecord> AddAsync(LifeRecord lifeRecord)
        {
            var entry=await _context.LifeRecords.AddAsync(lifeRecord);
            return entry.Entity;
        }

        public LifeRecord Update(LifeRecord lifeRecord)
        {
            var entry = _context.LifeRecords.Update(lifeRecord);
            return entry.Entity;
        }

        public async Task<LifeRecord> DeleteAsync(int id)
        {
            var record=await GetRecordByIdAsync(id);
            record.SetDeleted();
            return Update(record);
        }

        public Task<LifeRecord> GetRecordByIdAsync(int id)
        {
            return _context.LifeRecords.FirstAsync(l => l.Id == id);
        }
    }
}
