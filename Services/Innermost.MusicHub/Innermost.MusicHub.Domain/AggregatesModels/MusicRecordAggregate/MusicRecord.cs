using DomainSeedWork;
using Innermost.MusicHub.Domain.AggregatesModels.MusicRecordAggregate.Entities;

namespace Innermost.MusicHub.Domain.AggregatesModels.MusicRecordAggregate
{
    public class MusicRecord : TagableEntity<string>, IAggregateRoot
    {
        public int MusicId { get; private set; }
        public string MusicName { get; private set; }
        public string? TranslatedMusicName { get; private set; }
        public string? Introduction { get; private set; }
        public string Genre { get; private set; }
        public string Language { get; private set; }
        public string AlbumCoverUrl { get; private set; }
        public string MusicUrl { get; private set; }
        public string? WikiUrl { get; private set; }
        public string Lyric { get; private set; }

        private readonly List<MusicRecordSinger> _singers;
        public IReadOnlyCollection<MusicRecordSinger> Singers => _singers;

        public MusicRecordAlbum Album { get; private set; }

        private readonly List<IReferrer> _referrers;
        public IReadOnlyCollection<IReferrer> Referrers => _referrers;

        public string PublishTime { get; private set; }
        public MusicRecord(
            string mid,int musicId,
            string musicName, string? translatedMusicName,
            string genre, string language,
            string albumCoverUrl, string musicUrl, string? wikiUrl, string lyric,
            List<MusicRecordSinger> singers, MusicRecordAlbum album, string publishTime,
            List<IReferrer> referrers, List<TagSummary> tagSummaries) : base(tagSummaries)
        {
            Id = mid;
            MusicName = musicName;
            MusicId=musicId;
            TranslatedMusicName = translatedMusicName;
            Genre = genre;
            Language = language;
            AlbumCoverUrl = albumCoverUrl;
            MusicUrl = musicUrl;
            Lyric = lyric;
            _singers = (singers is not null || singers?.Count > 0) ? singers : throw new ArgumentException("Singers must have at least one singer and can not be null");
            Album = album ?? throw new ArgumentException("MusicRecord must contains in a album");
            PublishTime = publishTime;
            _referrers = referrers ?? new List<IReferrer>();
            WikiUrl = wikiUrl;
        }

        protected override IReferrer ToReferrer()
        {
            throw new NotImplementedException();
        }
    }
}
