using Grpc.Core;
using Innermost.MusicHub.API.Queries.MusicRecordQueries;

namespace Innermost.MusicHub.API.Grpc.Services
{
    public class MusicRecordGrpcService:MusicRecordGrpc.MusicRecordGrpcBase
    {
        private readonly IMusicRecordQueries _musicRecordQueries;
        public MusicRecordGrpcService(IMusicRecordQueries musicRecordQueries)
        {
            _musicRecordQueries=musicRecordQueries;
        }
        public override async Task<MusicRecordGrpcDTO> GetRandomMusicRecordByTag(MusicRecordTagGrpcDTO request, ServerCallContext context)
        {
            var music = await _musicRecordQueries.GetOneRandomMusicRecord(mr=>mr.Tags.Any(t=> request.TagName.Contains(t.TagName)));//useful test in TestMongoDBReplica.
            return new MusicRecordGrpcDTO()
            {
                Mid = music.Mid,
                MusicName = music.MusicName,
                MusicAlbum = music.Album.AlbumName,
                MusicSinger = string.Join(" / ", music.Singers)
            };
        }
    }
}
