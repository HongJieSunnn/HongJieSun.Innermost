using Innermost.Meet.Domain.AggregatesModels.UserInteractionAggregate;

namespace Innermost.Meet.Domain.Repositories
{
    public interface IUserInteractionRepository:IRepository<UserInteraction>
    {
        Task<UserInteraction> GetUserInteractionAsync(string interactiveUserId);

        Task AddUserInteractionAsync(UserInteraction userInteraction);

        Task<UpdateResult> UpdateUserInteractionAsync(string interactiveUserId,UpdateDefinition<UserInteraction> updateDefinition,params FilterDefinition<UserInteraction>[] filterDefinitions);
        Task<UpdateResult> UpdateManyUserInteractionsAsync(IEnumerable<string> interactiveUserIds,UpdateDefinition<UserInteraction> updateDefinition,params FilterDefinition<UserInteraction>[] filterDefinitions);
    }
}
