using Innermost.LogLife.Domain.AggregatesModels.LifeRecordAggregate.ValueObjects;

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
            var entry=_context.Update(lifeRecord);

            await AddLocationAsync(lifeRecord);
            await AddMusicRecordAsync(lifeRecord);
            await AddTagSummariesAsync(lifeRecord);

            return entry.Entity;
        }

        public LifeRecord Update(LifeRecord lifeRecord)
        {
            var entry = _context.LifeRecords.Update(lifeRecord);
            return entry.Entity;
        }

        public async Task<LifeRecord?> DeleteAsync(int id, string userId)
        {
            var records=await _context.LifeRecords.Where(l=>l.Id == id&&l.DeleteTime == null).Include(l=>l.Tags).ToListAsync();
            var record=records.FirstOrDefault(l=>l.UserId==userId);//if record is not existed.The First method will throw exceptions.
            record?.SetDeleted();
            return record;
        }

        public async Task<LifeRecord?> GetRecordByIdAsync(int id, string userId)
        {
            //if we return _context.LifeRecords.FirstOrDefaultAsync(l => l.Id == id&&l.UserId==userId)
            //there calls error : Translation of member 'UserId' on entity type 'LifeRecord' failed. This commonly occurs when the specified member is unmapped.
            //I guess that we mapped _userId field instead of UserId property,so efcore can not transalate that expression to SQL.see https://docs.microsoft.com/zh-cn/ef/core/querying/client-eval
            var records =await _context.LifeRecords.Where(l => l.Id == id).ToListAsync();
            return records.FirstOrDefault(r=>r.UserId== userId);
        }

        private async Task AddLocationAsync(LifeRecord lifeRecord)
        {
            if(lifeRecord.Location is not null)
            {
                var existed=await _context.Locations.ContainsAsync(lifeRecord.Location);

                if(!existed)
                {
                    var location = _context.ChangeTracker.Entries<Location>().First(e => e.Entity.Id == lifeRecord.LocationUId && e.State == EntityState.Modified);
                    var baiduPOI = _context.ChangeTracker.Entries<BaiduPOI>().First(e => e.Properties.FirstOrDefault(p => p.CurrentValue == lifeRecord.LocationUId) is not null && e.State == EntityState.Modified);

                    location.State = EntityState.Added;
                    baiduPOI.State = EntityState.Added;
                }
            }
        }

        private async Task AddMusicRecordAsync(LifeRecord lifeRecord)
        {
            if (lifeRecord.MusicRecord is not null)
            {
                var existed = await _context.MusicRecords.ContainsAsync(lifeRecord.MusicRecord);

                if (!existed)
                {
                    var musicRecord = _context.ChangeTracker.Entries<MusicRecord>().First(e => e.Entity.Id == lifeRecord.MusicRecordMId && e.State == EntityState.Modified);
                    
                    musicRecord.State = EntityState.Added;
                }
            }
        }

        private async Task AddTagSummariesAsync(LifeRecord lifeRecord)
        {
            var tagSummariesExisted =await _context.TagSummaries.Where(t => lifeRecord.Tags.Contains(t)).ToListAsync();
            var tagSummariesTrackers = _context.ChangeTracker.Entries<TagSummary<int, LifeRecord>>().Where(t => !tagSummariesExisted.Contains(t.Entity));

            foreach(var tagSummary in tagSummariesTrackers)
            {
                tagSummary.State= EntityState.Added;
            }
        }
    }
}
