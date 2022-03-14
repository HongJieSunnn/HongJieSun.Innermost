namespace Innermost.LogLife.API.Application.Commands
{
    public class DeleteRecordCommand:IRequest<bool>
    {
        public int RecordId { get; set; }
        public DeleteRecordCommand(int recordId)
        {
            RecordId=recordId;
        }
    }
}
