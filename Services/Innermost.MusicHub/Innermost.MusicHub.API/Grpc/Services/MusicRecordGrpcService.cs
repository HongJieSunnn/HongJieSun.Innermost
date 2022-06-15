using Grpc.Core;
using Innermost.MusicHub.API.Queries.MusicRecordQueries;
using Innermost.MusicHub.Domain.AggregatesModels.MusicRecordAggregate;
using MongoDB.Driver;
using TagS.Microservices.Client.Models;

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
            var filter = Builders<MusicRecord>.Filter.ElemMatch("Tags", Builders<TagSummary>.Filter.In("TagName", request.TagName));
            var music = await _musicRecordQueries.GetOneRandomMusicRecord(filter);
            return new MusicRecordGrpcDTO()
            {
                Mid = music.Mid,
                MusicName = music.MusicName,
                MusicAlbum = music.Album.AlbumName,
                MusicSinger = string.Join(" , ", music.Singers.Select(s=>s.SingerName)),
                MusicCoverUrl=music.AlbumCoverUrl,
            };
        }
    }
}
