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
        public MusicHubMongoDBContext(MongoDBContextConfiguration<MusicHubMongoDBContext> configuration) : base(configuration)
        {
            //TODO create indexes
        }

        public async Task<bool> SaveEntitiesAsync<TEntity>(TEntity entity, CancellationToken cancellationToken = default) where TEntity : Entity<string>
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
                base.Client.Cluster.Dispose();
                _disposed = true;
            }
        }
    }
}
