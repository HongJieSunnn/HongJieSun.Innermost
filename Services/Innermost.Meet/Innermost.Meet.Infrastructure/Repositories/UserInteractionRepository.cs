using Innermost.Meet.Domain.AggregatesModels.UserInteractionAggregate;
namespace Innermost.Meet.Infrastructure.Repositories
{
    public class UserInteractionRepository : IUserInteractionRepository
    {
        private readonly MeetMongoDBContext _context;
        private readonly IClientSessionHandle _session;
        public IUnitOfWork UnitOfWork => _context;

        public UserInteractionRepository(MeetMongoDBContext context,IClientSessionHandle session)
        {
            _context=context;
            _session=session;
        }

        public Task<UserInteraction> GetUserInteractionAsync(string interactiveUserId)
        {
            return _context.UserInteractions.Find(ui=>ui.UserId==interactiveUserId).FirstAsync();
        }

        public Task AddUserInteractionAsync(UserInteraction userInteraction)
        {
            return _context.UserInteractions.InsertOneAsync(_session, userInteraction);
        }

        public Task<UpdateResult> UpdateUserInteractionAsync(string interactiveUserId, UpdateDefinition<UserInteraction> updateDefinition, params FilterDefinition<UserInteraction>[] filterDefinitions)
        {
            var idFilter = Builders<UserInteraction>.Filter.Eq(ui => ui.UserId, interactiveUserId);
            var filter = filterDefinitions.CombineFilterDefinitions(idFilter);

            return _context.UserInteractions.UpdateOneAsync(_session, filter, updateDefinition);
        }

        public Task<UpdateResult> UpdateManyUserInteractionsAsync(IEnumerable<string> interactiveUserIds, UpdateDefinition<UserInteraction> updateDefinition, params FilterDefinition<UserInteraction>[] filterDefinitions)
        {
            var idFilter=Builders<UserInteraction>.Filter.In(ui => ui.UserId, interactiveUserIds);
            var filter = filterDefinitions.CombineFilterDefinitions(idFilter);

            return _context.UserInteractions.UpdateManyAsync(_session,filter, updateDefinition);
        }
    }
}
