namespace Innermost.Meet.API.Application.Commands.SharedLifeRecordAggregate
{
    public class LikeSharedLifeRecordCommand:IRequest<bool>
    {
        public string SharedLifeRecordObjectId { get; set; }
        public string? LikerUserId { get; set; }
        public DateTime? LikeTime { get; set; }
        public LikeSharedLifeRecordCommand(string sharedLifeRecordObjectId,string? likerUserId,DateTime? likeTime)
        {
            SharedLifeRecordObjectId = sharedLifeRecordObjectId;
            LikerUserId = likerUserId;
            LikeTime = likeTime??DateTime.Now;
        }
    }
}
