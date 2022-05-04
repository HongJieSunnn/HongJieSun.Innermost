using Innermost.Meet.Domain.AggregatesModels.UserConfidantAggregate;

namespace Innermost.Meet.Infrastructure.Repositories
{
    public class UserSocialContactRepository : IUserSocialContactRepository
    {
        private readonly MeetMongoDBContext _context;
        public UserSocialContactRepository(MeetMongoDBContext context)
        {
            _context = context;

        }
        public IUnitOfWork UnitOfWork => _context;

        public Task<UserSocialContact> GetUserSocialContactAsync(string userSocialContactUserId)
        {
            return _context.UserSocialContacts.Find(usc => usc.UserId == userSocialContactUserId).FirstAsync();
        }

        public Task AddUserSocialContactAsync(UserSocialContact userSocialContact)
        {
            return _context.UserSocialContacts.InsertOneAsync(userSocialContact);
        }

        public Task<UpdateResult> UpdateUserSocialContactAsync(string userSocialContactUserId, UpdateDefinition<UserSocialContact> updateDefinition, params FilterDefinition<UserSocialContact>[] filterDefinitions)
        {
            var idFilter=Builders<UserSocialContact>.Filter.Eq(usc => usc.UserId, userSocialContactUserId);
            var filter = filterDefinitions.CombineFilterDefinitions(idFilter);

            return _context.UserSocialContacts.UpdateOneAsync(filter, updateDefinition);
        }

        public Task<UpdateResult> UpdateManyUserSocialContactsAsync(IEnumerable<string> userSocialContactUserIds, UpdateDefinition<UserSocialContact> updateDefinition, params FilterDefinition<UserSocialContact>[] filterDefinitions)
        {
            var idFilter = Builders<UserSocialContact>.Filter.In(usc => usc.UserId, userSocialContactUserIds);
            var filter = filterDefinitions.CombineFilterDefinitions(idFilter);

            return _context.UserSocialContacts.UpdateOneAsync(filter, updateDefinition);
        }
    }
}
