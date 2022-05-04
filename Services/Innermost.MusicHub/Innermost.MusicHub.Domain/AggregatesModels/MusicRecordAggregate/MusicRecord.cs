using DomainSeedWork;
using Innermost.MusicHub.Domain.AggregatesModels.MusicRecordAggregate.Entities;

namespace Innermost.MusicHub.Domain.AggregatesModels.MusicRecordAggregate
{
    public class MusicRecord : TagableEntity<string>, IAggregateRoot
    {
        public string MusicMid { get; private set; }
        public long MusicId { get; private set; }
        public string MusicName { get; private set; }
        public string? TranslatedMusicName { get; private set; }
        public string? Introduction { get; private set; }
        public string Genre { get; private set; }
        public string Language { get; private set; }
        public string AlbumCoverUrl { get; private set; }
        public string MusicUrl { get; private set; }
        public string? WikiUrl { get; private set; }
        public string Lyric { get; private set; }

        [BsonRequired]
        [BsonElement("Singers")]
        private List<MusicRecordSinger> _singers;
        public IReadOnlyCollection<MusicRecordSinger> Singers => _singers;

        public MusicRecordAlbum Album { get; private set; }

        public string PublishTime { get; private set; }
        public MusicRecord(
            string mid, long musicId,
            string musicName, string? translatedMusicName,
            string genre, string language,
            string albumCoverUrl, string musicUrl, string? wikiUrl, string lyric,
            List<MusicRecordSinger> singers, MusicRecordAlbum album, string publishTime,
            List<TagSummary> tagSummaries) : base(tagSummaries)
        {
            MusicMid = mid;
            MusicName = musicName;
            MusicId = musicId;
            TranslatedMusicName = translatedMusicName;
            Genre = genre;
            Language = language;
            AlbumCoverUrl = albumCoverUrl;
            MusicUrl = musicUrl;
            Lyric = lyric;
            _singers = (singers is not null || singers?.Count > 0) ? singers : throw new ArgumentException("Singers must have at least one singer and can not be null");
            Album = album ?? throw new ArgumentException("MusicRecord must contains in a album");
            PublishTime = publishTime;
            WikiUrl = wikiUrl;
        }

        protected override IReferrer ToReferrer()
        {
            throw new NotImplementedException();//Not as referrer in TagWithReferrer.Just storage TagSummary in MusicRecord.
        }
    }
}
