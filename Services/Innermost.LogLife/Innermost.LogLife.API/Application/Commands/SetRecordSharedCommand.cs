namespace Innermost.LogLife.API.Application.Commands
{
    public class SetRecordSharedCommand:IRequest<bool>
    {
        public int RecordId { get; set; }
        public SetRecordSharedCommand(int recordId)
        {
            RecordId = recordId;
        }
    }
}
