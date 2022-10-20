namespace Innermost.LogLife.API.Application.Commands
{
    public class DeleteRecordCommand : IRequest<bool>
    {
        public int RecordId { get; set; }
        public string? UserId { get; set; }
        public DeleteRecordCommand(int recordId, string? userId)
        {
            RecordId = recordId;
            UserId = userId;
        }
    }
}
