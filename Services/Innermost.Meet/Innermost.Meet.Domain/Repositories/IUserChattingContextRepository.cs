using Innermost.Meet.Domain.AggregatesModels.UserChattingAggregate;

namespace Innermost.Meet.Domain.Repositories
{
    public interface IUserChattingContextRepository:IRepository<UserChattingContext>
    {
        Task<UserChattingContext> GetUserChattingContextAsync(string chattingContextId);

        Task AddUserChattingContextAsync(UserChattingContext userChattingContext);

        Task<UpdateResult> UpdateUserChattingContextAsync(string chattingContextId, UpdateDefinition<UserChattingContext> updateDefinition, params FilterDefinition<UserChattingContext>[] filterDefinitions);
    }
}
