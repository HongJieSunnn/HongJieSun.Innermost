namespace Innermost.LogLife.API.Application.Commands
{
    public class SetRecordSharedCommand : IRequest<bool>
    {
        public int RecordId { get; set; }
        public string? UserId { get; set; }
        public SetRecordSharedCommand(int recordId, string userId)
        {
            RecordId = recordId;
            UserId = userId;
        }
    }
}
