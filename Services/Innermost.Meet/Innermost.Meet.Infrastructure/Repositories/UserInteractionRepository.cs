using Innermost.Meet.Domain.AggregatesModels.UserInteraction;
namespace Innermost.Meet.Infrastructure.Repositories
{
    public class UserInteractionRepository : IUserInteractionRepository
    {
        private readonly MeetMongoDBContext _context;

        public IUnitOfWork UnitOfWork => _context;

        public UserInteractionRepository(MeetMongoDBContext context)
        {
            _context=context;
        }

        public Task<UserInteraction> GetUserInteractionAsync(string interactiveUserId)
        {
            return _context.UserInteractions.Find(ui=>ui.Id==interactiveUserId).FirstAsync();
        }

        public Task AddUserInteractionAsync(UserInteraction userInteraction)
        {
            return _context.UserInteractions.InsertOneAsync(userInteraction);
        }

        public Task<UpdateResult> UpdateUserInteractionAsync(string interactiveUserId, UpdateDefinition<UserInteraction> updateDefinition, params FilterDefinition<UserInteraction>[] filterDefinitions)
        {
            var idFilter = Builders<UserInteraction>.Filter.Eq("_id", interactiveUserId);
            var filter = filterDefinitions.CombineFilterDefinitions(idFilter);

            return _context.UserInteractions.UpdateOneAsync(filter, updateDefinition);
        }

        public Task<UpdateResult> UpdateManyUserInteractionsAsync(IEnumerable<string> interactiveUserIds, UpdateDefinition<UserInteraction> updateDefinition, params FilterDefinition<UserInteraction>[] filterDefinitions)
        {
            var idFilter=Builders<UserInteraction>.Filter.In("_id",interactiveUserIds);
            var filter = filterDefinitions.CombineFilterDefinitions(idFilter);

            return _context.UserInteractions.UpdateManyAsync(filter, updateDefinition);
        }
    }
}
