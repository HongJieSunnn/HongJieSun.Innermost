using Innermost.Meet.Domain.AggregatesModels.UserChattingAggregate;
using Innermost.Meet.Domain.AggregatesModels.UserConfidantAggregate;
using Innermost.Meet.Domain.AggregatesModels.UserInteraction;
using Innermost.MongoDBContext;
using Innermost.MongoDBContext.Configurations;
using MediatR;

namespace Innermost.Meet.Infrastructure
{
    public class MeetMongoDBContext : MongoDBContextBase, IUnitOfWork
    {
        private bool _disposed;
        private readonly IMediator _mediator;
        public IMongoCollection<SharedLifeRecord> SharedLifeRecords { get; set; }
        public IMongoCollection<UserInteraction> UserInteractions { get; set; }
        public IMongoCollection<UserSocialContact> UserSocialContacts { get; set; }
        public IMongoCollection<UserChattingContext> UserChattingContexts { get; set; }
        public MeetMongoDBContext(MongoDBContextConfiguration<MeetMongoDBContext> contextConfiguration) : base(contextConfiguration)
        {

        }

        public MeetMongoDBContext(MongoDBContextConfiguration<MeetMongoDBContext> contextConfiguration,IMediator mediator) : base(contextConfiguration)
        {
            _mediator=mediator;
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

                entity.ClearDomainEvents();//reference
            }

            return true;
        }

        public void Dispose()
        {
            if (!_disposed)
            {
                ClusterRegistry.Instance.UnregisterAndDisposeCluster(Client.Cluster);//Actually,this is unnecessary.
                _disposed = true;
            }
        }

        public Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<bool> SaveEntitiesAsync(CancellationToken cancellationToken = default, bool saveChanges = true)
        {
            throw new NotImplementedException();
        }
    }
}
