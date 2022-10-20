using Innermost.MusicHub.Domain.AggregatesModels.AlbumAggregate;
using Innermost.MusicHub.Domain.AggregatesModels.MusicRecordAggregate;
using Innermost.MusicHub.Domain.AggregatesModels.SingerAggregate;
using MediatR;

namespace Innermost.MusicHub.Infrastructure
{
    public class MusicHubMongoDBContext : MongoDBContextBase, IUnitOfWork
    {
        private bool _disposed;
        private readonly IMediator _mediator;
        public IMongoCollection<Album> Albums { get; set; }
        public IMongoCollection<Singer> Singers { get; set; }
        public IMongoCollection<MusicRecord> MusicRecords { get; set; }
        public MusicHubMongoDBContext(MongoDBContextConfiguration<MusicHubMongoDBContext> configuration, IMediator mediator) : base(configuration)
        {
            _mediator = mediator;
            CreateIndexes();
        }

        void CreateIndexes()
        {
            CreateIndexesForAlbums();
            CreateIndexesForSingers();
            CreateIndexesForMusicRecords();
        }

        void CreateIndexesForAlbums()
        {
            if (!Albums!.Indexes.List().Any())
            {
                var albumNameIndex = Builders<Album>.IndexKeys.Ascending(t => t.AlbumName);
                var albumGenreIndex = Builders<Album>.IndexKeys.Ascending(t => t.AlbumGenre);
                var albumMidIndex = Builders<Album>.IndexKeys.Ascending(t => t.AlbumMid);
                var publishTimeIndex = Builders<Album>.IndexKeys.Descending(t => t.PublishTime);

                var createUniqueIndexModels = new[] { albumMidIndex }.Select(al => new CreateIndexModel<Album>(al, new CreateIndexOptions() { Unique = true }));
                var createIndexModels = new[] { albumNameIndex, albumGenreIndex, publishTimeIndex }.Select(al => new CreateIndexModel<Album>(al)).ToList();

                createIndexModels.AddRange(createUniqueIndexModels);
                if (createIndexModels.Any())
                    Albums.Indexes.CreateManyAsync(createIndexModels).GetAwaiter().GetResult();
            }
        }

        void CreateIndexesForSingers()
        {
            if (!Singers!.Indexes.List().Any())
            {
                var singerNameIndex = Builders<Singer>.IndexKeys.Ascending(s => s.SingerName);
                var singerRegionIndex = Builders<Singer>.IndexKeys.Ascending(s => s.SingerRegion);
                var singerMidIndex = Builders<Singer>.IndexKeys.Ascending(s => s.SingerMid);

                var createUniqueIndexModels = new[] { singerMidIndex }.Select(al => new CreateIndexModel<Singer>(al, new CreateIndexOptions() { Unique = true }));
                var createIndexModels = new[] { singerNameIndex, singerRegionIndex }.Select(al => new CreateIndexModel<Singer>(al)).ToList();

                createIndexModels.AddRange(createUniqueIndexModels);
                if (createIndexModels.Any())
                    Singers.Indexes.CreateManyAsync(createIndexModels).GetAwaiter().GetResult();
            }
        }

        void CreateIndexesForMusicRecords()
        {
            if (!MusicRecords!.Indexes.List().Any())
            {
                var musicMidIndex = Builders<MusicRecord>.IndexKeys.Ascending(m => m.MusicMid);
                var musicNameIndex = Builders<MusicRecord>.IndexKeys.Ascending(m => m.MusicName);
                var musicGenreIndex = Builders<MusicRecord>.IndexKeys.Ascending(m => m.Genre);
                var musicLanguageIndex = Builders<MusicRecord>.IndexKeys.Ascending(m => m.Language);
                var publishTimeIndex = Builders<MusicRecord>.IndexKeys.Descending(m => m.PublishTime);
                var musicTagIdIndex = Builders<MusicRecord>.IndexKeys.Ascending("Tags.TagId");//MusicRecords do not store in TagWithReferrer so need indexes.
                var musicTagNameIndex = Builders<MusicRecord>.IndexKeys.Ascending("Tags.TagName");
                var musicSingerNameIndex = Builders<MusicRecord>.IndexKeys.Ascending("Singers.SingerName");

                var createUniqueIndexModels = new[] { musicMidIndex }.Select(al => new CreateIndexModel<MusicRecord>(al, new CreateIndexOptions() { Unique = true }));
                var createIndexModels = new[] { musicNameIndex, musicGenreIndex, musicLanguageIndex, musicTagIdIndex, musicTagNameIndex, publishTimeIndex, musicSingerNameIndex }.Select(i => new CreateIndexModel<MusicRecord>(i)).ToList();

                createIndexModels.AddRange(createUniqueIndexModels);
                if (createIndexModels.Any())
                    MusicRecords.Indexes.CreateManyAsync(createIndexModels).GetAwaiter().GetResult();
            }
        }

        public async Task<bool> SaveEntitiesAsync<TEntity>(TEntity entity, CancellationToken cancellationToken = default) where TEntity : Entity<string>, IAggregateRoot
        {
            if (entity.DomainEvents is not null)
            {
                var domainEvents = entity.DomainEvents;

                foreach (var domainEvent in domainEvents)
                {
                    await _mediator.Publish(domainEvent);
                }

                entity.ClearDomainEvents();
            }

            return true;
        }

        public Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<bool> SaveEntitiesAsync(CancellationToken cancellationToken = default, bool saveChanges = true)
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
            if (!_disposed)
            {
                ClusterRegistry.Instance.UnregisterAndDisposeCluster(Client.Cluster);
                _disposed = true;
            }
        }
    }
}
