using Innermost.Meet.Domain.AggregatesModels.UserInteraction;

namespace Innermost.Meet.Domain.Repositories
{
    public interface IUserInteractionRepository:IRepository<UserInteraction>
    {
        Task<UserInteraction> GetUserInteractionAsync(string interactiveUserId);

        Task<UpdateResult> UpdateUserInteractionAsync(string interactiveUserId,UpdateDefinition<UserInteraction> updateDefinition,params FilterDefinition<UserInteraction>[] filterDefinitions);
        Task<UpdateResult> UpdateManyUserInteractionsAsync(IEnumerable<string> interactiveUserIds,UpdateDefinition<UserInteraction> updateDefinition,params FilterDefinition<UserInteraction>[] filterDefinitions);
    }
}
