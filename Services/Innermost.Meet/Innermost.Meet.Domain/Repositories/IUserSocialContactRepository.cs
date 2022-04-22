using Innermost.Meet.Domain.AggregatesModels.UserConfidantAggregate;

namespace Innermost.Meet.Domain.Repositories
{
    public interface IUserSocialContactRepository:IRepository<UserSocialContact>
    {
        Task<UserSocialContact> GetUserSocialContactAsync(string userId);
        Task<UpdateResult> UpdateUserSocialContactAsync(string userId, UpdateDefinition<UserSocialContact> updateDefinition, params FilterDefinition<UserSocialContact>[] filterDefinitions);
        Task<UpdateResult> UpdateManyUserSocialContactsAsync(IEnumerable<string> userIds, UpdateDefinition<UserSocialContact> updateDefinition, params FilterDefinition<UserSocialContact>[] filterDefinitions);
    }
}
