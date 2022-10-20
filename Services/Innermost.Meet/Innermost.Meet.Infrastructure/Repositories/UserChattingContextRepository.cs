using Innermost.Meet.Domain.AggregatesModels.UserChattingAggregate;

namespace Innermost.Meet.Infrastructure.Repositories
{
    public class UserChattingContextRepository : IUserChattingContextRepository
    {
        private readonly MeetMongoDBContext _context;
        private readonly IClientSessionHandle _session;

        public IUnitOfWork UnitOfWork => _context;

        public UserChattingContextRepository(MeetMongoDBContext context, IClientSessionHandle session)
        {
            _context = context;
            _session = session;
        }

        public Task<UserChattingContext> GetUserChattingContextAsync(string chattingContextId)
        {
            return _context.UserChattingContexts.Find(ucc => ucc.Id == chattingContextId).FirstAsync();
        }

        public Task AddUserChattingContextAsync(UserChattingContext userChattingContext)
        {
            return _context.UserChattingContexts.InsertOneAsync(_session, userChattingContext);
        }

        public Task<UpdateResult> UpdateUserChattingContextAsync(string chattingContextId, UpdateDefinition<UserChattingContext> updateDefinition, params FilterDefinition<UserChattingContext>[] filterDefinitions)
        {
            var chattingContextIdFilter = Builders<UserChattingContext>.Filter.Eq(uc => uc.Id, chattingContextId);

            var filter = filterDefinitions.CombineFilterDefinitions(chattingContextIdFilter);

            return _context.UserChattingContexts.UpdateOneAsync(_session, filter, updateDefinition);
        }
    }
}
