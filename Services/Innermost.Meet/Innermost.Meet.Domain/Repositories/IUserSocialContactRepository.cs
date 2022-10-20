using Innermost.Meet.Domain.AggregatesModels.UserConfidantAggregate;

namespace Innermost.Meet.Domain.Repositories
{
    public interface IUserSocialContactRepository : IRepository<UserSocialContact>
    {
        Task<UserSocialContact> GetUserSocialContactAsync(string userSocialContactUserId);

        Task AddUserSocialContactAsync(UserSocialContact userSocialContact);

        Task<UpdateResult> UpdateUserSocialContactAsync(string userSocialContactUserId, UpdateDefinition<UserSocialContact> updateDefinition, params FilterDefinition<UserSocialContact>[] filterDefinitions);
        Task<UpdateResult> UpdateManyUserSocialContactsAsync(IEnumerable<string> userSocialContactUserIds, UpdateDefinition<UserSocialContact> updateDefinition, params FilterDefinition<UserSocialContact>[] filterDefinitions);
    }
}
