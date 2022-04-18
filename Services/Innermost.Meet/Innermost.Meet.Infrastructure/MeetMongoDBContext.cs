using Innermost.MongoDBContext;
using Innermost.MongoDBContext.Configurations;


namespace Innermost.Meet.Infrastructure
{
    public class MeetMongoDBContext : MongoDBContextBase, IUnitOfWork
    {
        private bool _disposed;
        public IMongoCollection<SharedLifeRecord> SharedLifeRecords { get; set; }
        public MeetMongoDBContext(MongoDBContextConfiguration<MeetMongoDBContext> contextConfiguration) : base(contextConfiguration)
        {

        }
        public Task<bool> SaveEntitiesAsync<TEntity>(TEntity entity, CancellationToken cancellationToken = default) where TEntity : Entity<string>
        {
            throw new NotImplementedException();
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
