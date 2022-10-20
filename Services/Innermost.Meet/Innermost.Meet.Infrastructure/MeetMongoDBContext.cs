using Innermost.Meet.Domain.AggregatesModels.UserChattingAggregate;
using Innermost.Meet.Domain.AggregatesModels.UserConfidantAggregate;
using Innermost.Meet.Domain.AggregatesModels.UserInteractionAggregate;
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

        public MeetMongoDBContext(MongoDBContextConfiguration<MeetMongoDBContext> contextConfiguration, IMediator mediator) : base(contextConfiguration)
        {
            _mediator = mediator;
            CreateIndexes();
        }

        void CreateIndexes()
        {
            CreateIndexesForSharedLifeRecords();
            CreateIndexesForUserInteractions();
            CreateIndexesForUserSocialContact();
            CreateIndexesForUserChattingContext();
        }

        void CreateIndexesForSharedLifeRecords()
        {
            if (!SharedLifeRecords!.Indexes.List().Any())
            {
                var recordIdIndex = Builders<SharedLifeRecord>.IndexKeys.Ascending(sl => sl.RecordId);
                var userIdIndex = Builders<SharedLifeRecord>.IndexKeys.Ascending(sl => sl.UserId);
                var textIndex = Builders<SharedLifeRecord>.IndexKeys.Ascending(sl => sl.Text);
                var locationUidIndex = Builders<SharedLifeRecord>.IndexKeys.Ascending("Location.LocationUid");
                var locationIndex = Builders<SharedLifeRecord>.IndexKeys.Geo2DSphere("Location.BaiduPOI");
                var musicMidIndex = Builders<SharedLifeRecord>.IndexKeys.Ascending("MusicRecord.MusicMid");
                var createTimeIndex = Builders<SharedLifeRecord>.IndexKeys.Ascending(sl => sl.CreateTime);
                var likecountIndex = Builders<SharedLifeRecord>.IndexKeys.Descending(sl => sl.LikesCount);



                var createUniqueIndexModels = new[] { recordIdIndex }.Select(al => new CreateIndexModel<SharedLifeRecord>(al, new CreateIndexOptions() { Unique = true }));
                var createIndexModels = new[] { userIdIndex, textIndex, locationIndex, createTimeIndex, locationUidIndex, musicMidIndex, likecountIndex }.Select(al => new CreateIndexModel<SharedLifeRecord>(al)).ToList();

                createIndexModels.AddRange(createUniqueIndexModels);
                if (createIndexModels.Any())
                    SharedLifeRecords.Indexes.CreateManyAsync(createIndexModels).GetAwaiter().GetResult();
            }
        }

        void CreateIndexesForUserInteractions()
        {
            if (!UserInteractions!.Indexes.List().Any())
            {
                var userIdIndex = Builders<UserInteraction>.IndexKeys.Ascending(sl => sl.UserId);

                var createUniqueIndexModels = new[] { userIdIndex }.Select(al => new CreateIndexModel<UserInteraction>(al, new CreateIndexOptions() { Unique = true }));
                var createIndexModels = new IndexKeysDefinition<UserInteraction>[] { }.Select(al => new CreateIndexModel<UserInteraction>(al)).ToList();

                createIndexModels.AddRange(createUniqueIndexModels);
                if (createIndexModels.Any())
                    UserInteractions.Indexes.CreateManyAsync(createIndexModels).GetAwaiter().GetResult();
            }
        }

        void CreateIndexesForUserSocialContact()
        {
            if (!UserSocialContacts!.Indexes.List().Any())
            {
                var userIdIndex = Builders<UserSocialContact>.IndexKeys.Ascending(sl => sl.UserId);

                var createUniqueIndexModels = new[] { userIdIndex }.Select(al => new CreateIndexModel<UserSocialContact>(al, new CreateIndexOptions() { Unique = true }));
                var createIndexModels = new IndexKeysDefinition<UserSocialContact>[] { }.Select(al => new CreateIndexModel<UserSocialContact>(al)).ToList();

                createIndexModels.AddRange(createUniqueIndexModels);
                if (createIndexModels.Any())
                    UserSocialContacts.Indexes.CreateManyAsync(createIndexModels).GetAwaiter().GetResult();
            }
        }

        void CreateIndexesForUserChattingContext()
        {
            if (!UserChattingContexts!.Indexes.List().Any())
            {
                var usersIndex = Builders<UserChattingContext>.IndexKeys.Ascending("Users");

                var createUniqueIndexModels = new IndexKeysDefinition<UserChattingContext>[] { }.Select(al => new CreateIndexModel<UserChattingContext>(al, new CreateIndexOptions()
                {
                    Unique = true,
                }));
                var createIndexModels = new IndexKeysDefinition<UserChattingContext>[] { usersIndex }.Select(al => new CreateIndexModel<UserChattingContext>(al)).ToList();

                createIndexModels.AddRange(createUniqueIndexModels);

                if (createIndexModels.Any())
                    UserChattingContexts.Indexes.CreateManyAsync(createIndexModels).GetAwaiter().GetResult();
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
