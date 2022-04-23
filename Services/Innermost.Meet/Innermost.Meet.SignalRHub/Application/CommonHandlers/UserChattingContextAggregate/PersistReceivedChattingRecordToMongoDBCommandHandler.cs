using Innermost.Meet.Domain.AggregatesModels.UserChattingAggregate.Entities;

namespace Innermost.Meet.SignalRHub.Application.CommonHandlers.UserChattingContextAggregate
{
    public class PersistReceivedChattingRecordToMongoDBCommandHandler : IRequestHandler<PersistReceivedChattingRecordToMongoDBCommand, bool>
    {
        private readonly IUserChattingContextRepository _userChattingContextRepository;
        public PersistReceivedChattingRecordToMongoDBCommandHandler(IUserChattingContextRepository userChattingContextRepository)
        {
            _userChattingContextRepository=userChattingContextRepository;
        }
        public async Task<bool> Handle(PersistReceivedChattingRecordToMongoDBCommand request, CancellationToken cancellationToken)
        {
            var userChattingContext=await _userChattingContextRepository.GetUserChattingContextAsync(request.ChattingContextId);

            var update = userChattingContext.AddManyChattingRecords(request.ChattingRecordDTOs.Select(crdto => MapToChattingRecord(crdto)));

            var updateResult = await _userChattingContextRepository.UpdateUserChattingContextAsync(request.ChattingContextId, update);

            return true;
        }

        private ChattingRecord MapToChattingRecord(ChattingRecordDTO chattingRecordDTO)
        {
            return new ChattingRecord(
                chattingRecordDTO.SendUserId, 
                chattingRecordDTO.RecordMessage, 
                chattingRecordDTO.CreateTime, 
                chattingRecordDTO.TagSummaries.Select(tsdto=>new TagS.Microservices.Client.Models.TagSummary(tsdto.TagId,tsdto.TagName)).ToList()
            );
        }
    }
}
