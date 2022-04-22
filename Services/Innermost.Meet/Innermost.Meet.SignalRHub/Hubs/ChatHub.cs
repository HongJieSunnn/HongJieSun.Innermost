using Innermost.Identity.API;
using Innermost.Meet.Domain.Repositories;
using Microsoft.AspNetCore.Authorization;

namespace Innermost.Meet.SignalRHub.Hubs
{
    [Authorize]
    public class ChatHub:Hub
    {
        private readonly IdentityUserStatueGrpc.IdentityUserStatueGrpcClient _identityUserStatueGrpcClient;
        private readonly IUserChattingContextRepository _userChattingContextRepository;
        public ChatHub(IdentityUserStatueGrpc.IdentityUserStatueGrpcClient identityUserStatueGrpcClient,IUserChattingContextRepository userChattingContextRepository)
        {
            _identityUserStatueGrpcClient= identityUserStatueGrpcClient;
            _userChattingContextRepository= userChattingContextRepository;
        }

        public async Task SendMessageToUser(string toUserId,string message)
        {
            var sendUserId=GetConnectedUserId();

            

            if (!IsUserOnline(toUserId))
                return;//TODO store message redis and then to mongodb.
            await Clients.User(toUserId).SendAsync("SendMessage",message,sendUserId,DateTime.Now);
        }

        private bool IsUserOnline(string toUserId)
        {
            return _identityUserStatueGrpcClient.IsUserOnline(new IsUserOnlineUserIdGrpcDTO() { UserId = toUserId }).IsOnline;
        }

        private string GetConnectedUserId()
        {
            var connectUserId = Context.UserIdentifier;
            
            return connectUserId??throw new InvalidOperationException("User can not be null");
        }

        public override async Task OnConnectedAsync()
        {
            var connectedUserId = GetConnectedUserId();
            await _identityUserStatueGrpcClient.SetUserOnlineStatueAsync(new SetUserOnlineStatueGrpcDTO { UserId=connectedUserId,IsOnline = true });
            await base.OnConnectedAsync();
        }
        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            var connectedUserId = GetConnectedUserId();
            await _identityUserStatueGrpcClient.SetUserOnlineStatueAsync(new SetUserOnlineStatueGrpcDTO { UserId = connectedUserId, IsOnline = false });
            await base.OnDisconnectedAsync(exception);
        }
    }
}
