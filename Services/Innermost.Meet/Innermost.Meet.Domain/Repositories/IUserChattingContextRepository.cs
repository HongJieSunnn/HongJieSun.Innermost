using Innermost.Meet.Domain.AggregatesModels.UserChattingAggregate;

namespace Innermost.Meet.Domain.Repositories
{
    public interface IUserChattingContextRepository
    {
        Task<UserChattingContext> GetUserChattingContextAsync(string chattingContextId);

        Task<UpdateResult> UpdateUserChattingContextAsync(string chattingContextId, UpdateDefinition<UserChattingContext> updateDefinition, params FilterDefinition<UserChattingContext>[] filterDefinitions);
    }
}
