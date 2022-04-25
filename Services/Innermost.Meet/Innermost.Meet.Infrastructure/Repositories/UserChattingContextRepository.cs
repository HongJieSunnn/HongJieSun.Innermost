using IEnumerableExtensions;
using Innermost.Meet.Domain.AggregatesModels.UserChattingAggregate;

namespace Innermost.Meet.Infrastructure.Repositories
{
    public class UserChattingContextRepository : IUserChattingContextRepository
    {
        private readonly MeetMongoDBContext _context;

        public IUnitOfWork UnitOfWork => _context;

        public UserChattingContextRepository(MeetMongoDBContext context)
        {
            _context = context;

        }

        public Task<UserChattingContext> GetUserChattingContextAsync(string chattingContextId)
        {
            return _context.UserChattingContexts.Find(ucc=>ucc.Id==chattingContextId).FirstAsync();
        }

        public Task AddUserChattingContextAsync(UserChattingContext userChattingContext)
        {
            return _context.UserChattingContexts.InsertOneAsync(userChattingContext);
        }

        public Task<UpdateResult> UpdateUserChattingContextAsync(string chattingContextId, UpdateDefinition<UserChattingContext> updateDefinition, params FilterDefinition<UserChattingContext>[] filterDefinitions)
        {
            var chattingContextIdFilter = Builders<UserChattingContext>.Filter.Eq("_id", chattingContextId);

            var filter=filterDefinitions.CombineFilterDefinitions(chattingContextIdFilter);

            return _context.UserChattingContexts.UpdateOneAsync(filter, updateDefinition);
        }
    }
}
