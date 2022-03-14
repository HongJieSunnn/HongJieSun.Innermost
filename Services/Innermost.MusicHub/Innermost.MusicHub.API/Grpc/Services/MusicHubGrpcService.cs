using Grpc.Core;

namespace Innermost.MusicHub.API.Grpc.Services
{
    public class MusicHubGrpcService:MusicHubGrpc.MusicHubGrpcBase
    {
        public override Task<MusicDetailDTO> GetMusicDetail(MusicRecordDTO request, ServerCallContext context)
        {
            //TODO
            return base.GetMusicDetail(request, context);
        }
    }
}
