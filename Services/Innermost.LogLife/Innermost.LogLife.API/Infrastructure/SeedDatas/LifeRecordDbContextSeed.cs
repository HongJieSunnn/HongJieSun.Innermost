namespace Innemost.LogLife.API.Infrastructure.SeedDatas
{
    public class LifeRecordDbContextSeed
    {
        public async Task SeedAsync(LifeRecordDbContext context, IConfiguration configuration)
        {
            List<LifeRecord> seeders = new List<LifeRecord>();
            if (!context.LifeRecords.Any())
                seeders = GetDefaultLifeRecords();
            var locations = GetDefaultLocations().Where(l => !context.Locations.Contains(l));
            var musicRecords = GetDefaultMusicRecord().Where(l => !context.MusicRecords.Contains(l));
            //Location MusicRecord TextType 都不能够随 LifeRecord 添加到数据库，否则会出现冲突。
            //所以需要额外对它们进行添加，添加一条 LifeRecord 时只添加对应的 Id
            await context.Locations.AddRangeAsync(locations);
            await context.LifeRecords.AddRangeAsync(seeders);
            await context.MusicRecords.AddRangeAsync(musicRecords);
            

            await context.SaveChangesAsync();
        }

        List<LifeRecord> GetDefaultLifeRecords()
        {
            return new List<LifeRecord>();
        }

        List<Location> GetDefaultLocations()
        {
            return new List<Location>();
        }

        List<MusicRecord> GetDefaultMusicRecord()
        {
            return new List<MusicRecord>();
        }
    }
}
