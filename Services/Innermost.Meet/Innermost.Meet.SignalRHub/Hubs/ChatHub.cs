using Microsoft.AspNetCore.Authorization;

namespace Innermost.Meet.SignalRHub.Hubs
{
    [Authorize]
    public class ChatHub : Hub
    {
        private readonly IdentityUserStatueGrpc.IdentityUserStatueGrpcClient _identityUserStatueGrpcClient;
        private readonly IUserChattingContextQueries _userChattingContextQueries;
        private readonly IChattingRecordRedisService _chattingRecordRedisService;

        private const string AdminId = "13B8D30F-CFF8-20AB-8D40-1A64ADA8D067";
        public ChatHub(IdentityUserStatueGrpc.IdentityUserStatueGrpcClient identityUserStatueGrpcClient, IUserChattingContextQueries userChattingContextQueries, IChattingRecordRedisService chattingRecordRedisService)
        {
            _identityUserStatueGrpcClient = identityUserStatueGrpcClient;
            _chattingRecordRedisService = chattingRecordRedisService;
            _userChattingContextQueries = userChattingContextQueries;

        }

        public async Task SendMessageToUser(string toUserId, string chattingContextId, string message)
        {
            if (toUserId == AdminId)//Admin is all users' confidant,but Admin should never receive message.
                return;

            var sendUserId = GetConnectedUserId();
            if (!IsUserOnline(toUserId))
            {
                await _chattingRecordRedisService.AddChattingRecordAsync(chattingContextId, sendUserId, message, false);

                return;
            }

            var messageId = ObjectId.GenerateNewId().ToString();
            var chattingRecordDTO = new ChattingRecordDTO(messageId, sendUserId, message, DateTime.Now, null);

            await Clients.User(toUserId).SendAsync("ChattingMessage", chattingRecordDTO);

            await _chattingRecordRedisService.AddChattingRecordAsync(chattingContextId, chattingRecordDTO, true);
        }

        private bool IsUserOnline(string toUserId)
        {
            return _identityUserStatueGrpcClient.IsUserOnline(new IsUserOnlineUserIdGrpcDTO() { UserId = toUserId }).IsOnline;
        }

        private string GetConnectedUserId()
        {
            var connectUserId = Context.UserIdentifier;

            return connectUserId ?? throw new InvalidOperationException("User can not be null");
        }

        public override async Task OnConnectedAsync()
        {
            var connectedUserId = GetConnectedUserId();
            await _identityUserStatueGrpcClient.SetUserOnlineStatueAsync(new SetUserOnlineStatueGrpcDTO { UserId = connectedUserId, IsOnline = true });

            var chattingContextIds = await _userChattingContextQueries.GetAllChattingContextIdsOfUserAsync(connectedUserId);

            foreach (var chattingContextId in chattingContextIds)
            {
                var notReceivedMessages = await _chattingRecordRedisService.GetNotReceivedChattingRecordsAsync(chattingContextId, connectedUserId);
                var setNotReceivedChattingRecordsReceivedTask = _chattingRecordRedisService.SetNotReceivedChattingRecordsReceivedAsync(chattingContextId, notReceivedMessages.Count());

                foreach (var message in notReceivedMessages)
                {
                    await Clients.User(connectedUserId).SendAsync("NotReceviedChattingMessage", message);
                }

                await setNotReceivedChattingRecordsReceivedTask;
            }

            await base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            var connectedUserId = GetConnectedUserId();
            await _identityUserStatueGrpcClient.SetUserOnlineStatueAsync(new SetUserOnlineStatueGrpcDTO { UserId = connectedUserId, IsOnline = false });

            //persist records to MongoDB.
            var chattingContextIds = await _userChattingContextQueries.GetAllChattingContextIdsOfUserAsync(connectedUserId);
            var persistTasks = chattingContextIds.Select(c => _chattingRecordRedisService.PersistReceivedChattingRecordToMongoDBAsync(c));
            await Task.WhenAll(persistTasks);

            await base.OnDisconnectedAsync(exception);
        }
    }
}
