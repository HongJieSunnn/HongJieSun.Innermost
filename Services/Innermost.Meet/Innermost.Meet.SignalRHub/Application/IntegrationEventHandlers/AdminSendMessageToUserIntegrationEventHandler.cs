using Innermost.Meet.SignalRHub.Hubs;

namespace Innermost.Meet.SignalRHub.Application.IntegrationEventHandlers
{
    public class AdminSendMessageToUserIntegrationEventHandler : IIntegrationEventHandler<AdminSendMessageToUserIntegrationEvent>
    {
        private readonly IHubContext<ChatHub> _hubContext;
        private readonly IUserChattingContextQueries _userChattingContextQueries;
        private readonly IChattingRecordRedisService _chattingRecordRedisService;
        private readonly IdentityUserStatueGrpc.IdentityUserStatueGrpcClient _identityUserStatueGrpcClient;
        public AdminSendMessageToUserIntegrationEventHandler(
            IHubContext<ChatHub> hubContext,
            IUserChattingContextQueries userChattingContextQueries,
            IChattingRecordRedisService chattingRecordRedisService,
            IdentityUserStatueGrpc.IdentityUserStatueGrpcClient identityUserStatueGrpcClient)
        {
            _hubContext = hubContext;
            _userChattingContextQueries = userChattingContextQueries;
            _chattingRecordRedisService = chattingRecordRedisService;
            _identityUserStatueGrpcClient = identityUserStatueGrpcClient;
        }
        public async Task Handle(AdminSendMessageToUserIntegrationEvent @event)
        {
            var chattingContextId = await _userChattingContextQueries.GetChattingContextIdOfUsers(GetConnectedUserId(), @event.ToUserId);

            await SendMessageToUser(@event.ToUserId, chattingContextId, @event.Message);
        }

        private bool IsUserOnline(string toUserId)
        {
            return _identityUserStatueGrpcClient.IsUserOnline(new IsUserOnlineUserIdGrpcDTO() { UserId = toUserId }).IsOnline;
        }

        /// <summary>
        /// Consistent with ChatHub.SendMessageToUser.
        /// </summary>
        /// <param name="toUserId"></param>
        /// <param name="chattingContextId"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        private async Task SendMessageToUser(string toUserId, string chattingContextId, string message)
        {
            var sendUserId = GetConnectedUserId();
            if (!IsUserOnline(toUserId))
            {
                await _chattingRecordRedisService.AddChattingRecordAsync(chattingContextId, sendUserId, message, false);

                return;
            }

            var messageId = ObjectId.GenerateNewId().ToString();
            var chattingRecordDTO = new ChattingRecordDTO(messageId, sendUserId, message, DateTime.Now, null);

            await _hubContext.Clients.User(toUserId).SendAsync("ChattingMessage", chattingRecordDTO);

            await _chattingRecordRedisService.AddChattingRecordAsync(chattingContextId, chattingRecordDTO, true);
        }

        /// <summary>
        /// Get admin Id.
        /// </summary>
        /// <returns></returns>
        private string GetConnectedUserId()
        {
            return "13B8D30F-CFF8-20AB-8D40-1A64ADA8D067";
        }
    }
}
