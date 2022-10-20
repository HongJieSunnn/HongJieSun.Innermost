namespace Innermost.Meet.API.Application.DomainEventHandlers.SharedLifeRecordLikeAdded
{
    public class AddUserLikeToUserInteractionWhenSharedLifeRecordLikeAddedDomainEventHandler : INotificationHandler<SharedLifeRecordLikeAddedDomainEvent>
    {
        private readonly IUserInteractionRepository _userInteractionRepository;
        public AddUserLikeToUserInteractionWhenSharedLifeRecordLikeAddedDomainEventHandler(IUserInteractionRepository userInteractionRepository)
        {
            _userInteractionRepository = userInteractionRepository;
        }
        public async Task Handle(SharedLifeRecordLikeAddedDomainEvent notification, CancellationToken cancellationToken)
        {
            var userInteraction = await _userInteractionRepository.GetUserInteractionAsync(notification.LikerUserId);

            var recordLike = new RecordLike(
                notification.SharedLifeRecord.Id!,
                notification.SharedLifeRecord.UserId,
                notification.SharedLifeRecord.UserName, notification.SharedLifeRecord.UserNickName, notification.SharedLifeRecord.UserAvatarUrl,
                notification.SharedLifeRecord.Title, notification.SharedLifeRecord.Text,
                notification.SharedLifeRecord.MusicRecord is null ? null : $"{notification.SharedLifeRecord.MusicRecord.MusicName}-{notification.SharedLifeRecord.MusicRecord.Singer}",
                notification.SharedLifeRecord.Location is null ? null : notification.SharedLifeRecord.Location.Address,
                notification.SharedLifeRecord.CreateTime, notification.LikeTime);

            var update = userInteraction.AddRecordLike(recordLike);

            var updateResult = await _userInteractionRepository.UpdateUserInteractionAsync(notification.LikerUserId, update);

            var requestResult = updateResult.MatchedCount == 1 && updateResult.ModifiedCount == 1;
            if (!requestResult)
                throw new DomainEventHandleFailedException();//to roll back.
        }
    }
}
