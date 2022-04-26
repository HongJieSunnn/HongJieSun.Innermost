using Innermost.MusicHub.Domain.AggregatesModels.SingerAggregate;
using Innermost.MusicHub.Domain.AggregatesModels.MusicRecordAggregate;
using MediatR;
using Innermost.MusicHub.Domain.AggregatesModels.AlbumAggregate;

namespace Innermost.MusicHub.Infrastructure
{
    public class MusicHubMongoDBContext : MongoDBContextBase, IUnitOfWork
    {
        private bool _disposed;
        private readonly IMediator _mediator;
        public IMongoCollection<Album> Albums { get; set; }
        public IMongoCollection<Singer> Singers { get; set; }
        public IMongoCollection<MusicRecord> MusicRecords { get; set; }
        public MusicHubMongoDBContext(MongoDBContextConfiguration<MusicHubMongoDBContext> configuration,IMediator mediator) : base(configuration)
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
                var albumNameIndex= Builders<Album>.IndexKeys.Text(t => t.AlbumName);
                var albumGenreIndex= Builders<Album>.IndexKeys.Ascending(t => t.AlbumGenre);

                var createIndexModels = new[] { albumNameIndex, albumGenreIndex }.Select(al => new CreateIndexModel<Album>(al));

                Albums.Indexes.CreateManyAsync(createIndexModels).GetAwaiter().GetResult();
            }
        }

        void CreateIndexesForSingers()
        {
            if (!Singers!.Indexes.List().Any())
            {
                var singerNameIndex= Builders<Singer>.IndexKeys.Text(s=>s.SingerName);
                var singerRegionIndex= Builders<Singer>.IndexKeys.Ascending(s=>s.SingerRegion);

                var createIndexModels = new[] { singerNameIndex, singerRegionIndex }.Select(i => new CreateIndexModel<Singer>(i));

                Singers.Indexes.CreateManyAsync(createIndexModels).GetAwaiter().GetResult();
            }
        }

        void CreateIndexesForMusicRecords()
        {
            if (!MusicRecords!.Indexes.List().Any())
            {
                var musicNameIndex = Builders<MusicRecord>.IndexKeys.Text(m=>m.MusicName);
                var musicLyricIndex = Builders<MusicRecord>.IndexKeys.Text(m => m.Lyric);
                var musicGenreIndex = Builders<MusicRecord>.IndexKeys.Ascending(m=>m.Genre);
                var musicLanguageIndex = Builders<MusicRecord>.IndexKeys.Ascending(m=>m.Language);
                var musicTagIdIndex = Builders<MusicRecord>.IndexKeys.Ascending("Tags.TagId");
                var musicTagNameIndex = Builders<MusicRecord>.IndexKeys.Ascending("Tags.TagName");

                var createIndexModels = new[] { musicNameIndex, musicLyricIndex, musicGenreIndex, musicLanguageIndex, musicTagIdIndex, musicTagNameIndex }.Select(i => new CreateIndexModel<MusicRecord>(i));

                MusicRecords.Indexes.CreateManyAsync(createIndexModels).GetAwaiter().GetResult();
            }
        }

        public async Task<bool> SaveEntitiesAsync<TEntity>(TEntity entity, CancellationToken cancellationToken = default) where TEntity : Entity<string>,IAggregateRoot
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
