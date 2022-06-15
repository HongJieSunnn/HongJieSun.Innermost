using Innermost.Identity.API.UserStatue;

namespace Innermost.Meet.API.Queries.StatueQueries
{
    public class StatueQueries : IStatueQueries
    {
        private readonly IdentityUserStatueGrpc.IdentityUserStatueGrpcClient _identityUserStatueGrpcClient;
        public StatueQueries(IdentityUserStatueGrpc.IdentityUserStatueGrpcClient identityUserGrpcClient)
        {
            _identityUserStatueGrpcClient = identityUserGrpcClient;
        }
        public async Task<IEnumerable<StatueDTO>> GetManyUserStatuesAsync(IEnumerable<string> userIds)
        {
            var userIdsGrpcDTO = new UserIdsGrpcDTO();
            userIdsGrpcDTO.UserIds.AddRange(userIds);

            var onlineStatueTask = _identityUserStatueGrpcClient.GetUsersOnlineStatueAsync(userIdsGrpcDTO);
            var userStatueTask = _identityUserStatueGrpcClient.GetUsersStatueAsync(userIdsGrpcDTO);

            int userIdCount = userIds.Count();
            var statueDTOs = new List<StatueDTO>(userIdCount);

            var onlineStatues = (await onlineStatueTask).UsersOnlineStatues;
            var userStatues = (await userStatueTask).UsersStatues;

            for (int i = 0; i < userIdCount; i++)
            {
                statueDTOs.Add(new StatueDTO(onlineStatues[i], userStatues[i]));
            }

            return statueDTOs;
        }

        public async Task<StatueDTO> GetOneUserStatueAsync(string userId)
        {
            return (await GetManyUserStatuesAsync(new[] { userId })).First();
        }
    }
}
