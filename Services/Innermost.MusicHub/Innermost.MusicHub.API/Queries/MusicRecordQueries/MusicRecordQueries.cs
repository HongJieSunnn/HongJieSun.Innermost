using Innermost.MusicHub.API.Queries.MusicRecordQueries.Models;
using Innermost.MusicHub.Domain.AggregatesModels.MusicRecordAggregate;
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
        public async Task<MusicRecordDTO> GetOneRandomMusicRecord(Expression<Func<MusicRecord, bool>>? filter = null)
        {
            MusicRecord music;

            if (filter is not null)
                music = await _context.MusicRecords.AsQueryable().Where(filter).Sample(1).FirstAsync();
            else
                music= await _context.MusicRecords.AsQueryable().Sample(1).FirstAsync();


            return MapToMusicRecordDTO(music);
        }

        public async Task<IEnumerable<MusicRecordDTO>> SearchMusicRecordAsync(string musicRecordName)
        {
            var textFilter=Builders<MusicRecord>.Filter.Text(musicRecordName);

            return (await _context.MusicRecords.Find(textFilter).ToListAsync()).Select(m => MapToMusicRecordDTO(m));
        }

        private MusicRecordDTO MapToMusicRecordDTO(MusicRecord musicRecord)
        {
            return new MusicRecordDTO(
                musicRecord.Id!,
                musicRecord.MusicId,
                musicRecord.MusicName,
                musicRecord.TranslatedMusicName,
                musicRecord.Genre,
                musicRecord.Language,
                musicRecord.AlbumCoverUrl,
                musicRecord.MusicUrl,
                musicRecord.WikiUrl,
                musicRecord.Lyric,
                musicRecord.Singers.Select(s=>new MusicRecordSingerDTO(s.Id!,s.SingerName)).ToList(),
                new MusicRecordAlbumDTO(
                    musicRecord.Album.Id!,
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
                musicRecord.PublishTime
            );
        }
    }
}
