using Innermost.MusicHub.API.Queries.MusicRecordQueries.Models;
using Innermost.MusicHub.Domain.AggregatesModels.MusicRecordAggregate;
using Innermost.MusicHub.Domain.AggregatesModels.MusicRecordAggregate.Entities;
using System.Linq.Expressions;

namespace Innermost.MusicHub.API.Queries.MusicRecordQueries
{
    public class MusicRecordQueries : IMusicRecordQueries
    {
        private readonly MusicHubMongoDBContext _context;
        public MusicRecordQueries(MusicHubMongoDBContext context)
        {
            _context = context;

        }
        public async Task<MusicRecordDTO> GetOneRandomMusicRecord(FilterDefinition<MusicRecord>? filter = null)
        {
            MusicRecord music;

            if (filter is not null)
                music = await _context.MusicRecords.AsQueryable().Where(_=>filter.Inject()).Sample(1).FirstAsync();
            else
                music= await _context.MusicRecords.AsQueryable().Sample(1).FirstAsync();


            return MapToMusicRecordDTO(music);
        }

        public async Task<IEnumerable<MusicRecordDTO>> SearchMusicRecordAsync(string musicRecordName,int page=1,int limit=25)
        {
            var names=musicRecordName.Split('-');
            names = names.Select(n => n.Trim()).ToArray();
            var textFilter = Builders<MusicRecord>.Filter.Regex(mr => mr.MusicName, $"/^{names[0]}/i");
            if(names.Length > 1)
                //textFilter = textFilter & Builders<MusicRecord>.Filter.ElemMatch("Singers",Builders<MusicRecordSinger>.Filter.Regex(mrs=>mrs.SingerName, $"/^{names[1]}/i"));
                textFilter = textFilter & Builders<MusicRecord>.Filter.Eq("Singers.SingerName", $"{names[1]}");

            return (await _context.MusicRecords.Find(textFilter).Skip((page - 1) * limit).Limit(limit).ToListAsync()).OrderBy(t=>t.PublishTime).Select(m => MapToMusicRecordDTO(m));
        }

        private MusicRecordDTO MapToMusicRecordDTO(MusicRecord musicRecord)
        {
            return new MusicRecordDTO(
                musicRecord.MusicMid!,
                musicRecord.MusicId,
                musicRecord.MusicName,
                musicRecord.TranslatedMusicName,
                musicRecord.Introduction,
                musicRecord.Genre,
                musicRecord.Language,
                musicRecord.AlbumCoverUrl,
                musicRecord.MusicUrl,
                musicRecord.WikiUrl,
                musicRecord.Lyric,
                musicRecord.Singers.Select(s => new MusicRecordSingerDTO(s.Id!, s.SingerName)).ToList(),
                new MusicRecordAlbumDTO(
                    musicRecord.Album.AlbumMid!,
                    musicRecord.Album.AlbumName,
                    musicRecord.Album.AlbumDescriptions,
                    musicRecord.Album.AlbumGenre,
                    musicRecord.Album.AlbumLanguage,
                    musicRecord.Album.AlbumSingerName,
                    musicRecord.Album.AlbumSingerMid,
                    musicRecord.Album.AlbumSongCount,
                    musicRecord.Album.PublishCompany,
                    musicRecord.Album.PublishTime
                ),
                musicRecord.PublishTime,
                musicRecord.Tags.Select(t => new TagSummaryDTO(t.TagId, t.TagName)).ToList()
            );
        }
    }
}
