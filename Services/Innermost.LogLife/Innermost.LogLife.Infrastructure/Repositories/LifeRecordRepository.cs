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
            var addLocationTask=AddLocationAsync(lifeRecord);
            var addMusicRecordTask=AddMusicRecordAsync(lifeRecord);
            await addLocationTask;
            await addMusicRecordTask;

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
            var record=await GetRecordByIdAsync(id);//if id is not existed.The mothod GetRecordByIdAsync which call FirstAsync will throw exceptions.
            record.SetDeleted();
            return record;
        }

        public Task<LifeRecord> GetRecordByIdAsync(int id)
        {
            return _context.LifeRecords.FirstAsync(l => l.Id == id);
        }

        private async Task AddLocationAsync(LifeRecord lifeRecord)
        {
            if(lifeRecord.Location is not null)
            {
                var existed=await _context.Locations.ContainsAsync(lifeRecord.Location);

                if (existed)
                    lifeRecord.Location = null;//set null and will not be added by efcore and will not call duplicate primary error.
            }
        }

        private async Task AddMusicRecordAsync(LifeRecord lifeRecord)
        {
            if (lifeRecord.MusicRecord is not null)
            {
                var existed = await _context.MusicRecords.ContainsAsync(lifeRecord.MusicRecord);

                if (existed)
                    lifeRecord.MusicRecord = null;//set null and will not be added by efcore and will not call duplicate primary error.
            }
        }
    }
}
