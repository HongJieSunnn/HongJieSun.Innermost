using Innermost.MusicHub.Domain.AggregatesModels.AlbumAggregate.Entities;

namespace Innermost.MusicHub.Domain.AggregatesModels.AlbumAggregate
{
    public class Album : TagableEntity<string>, IAggregateRoot
    {
        public int AlbumId { get; private set; }
        public string AlbumName { get; private set; }
        public string AlbumDescriptions { get; private set; }
        public string AlbumGenre { get; private set; }
        public string AlbumLanguage { get; private set; }
        public int AlbumSongCount { get; private set; }
        public string AlbumCoverUrl { get; private set; }
        public string PublishCompany { get; private set; }
        public string PublishTime { get; private set; }

        private readonly List<AlbumMusicRecord> _musicRecords;
        public IReadOnlyCollection<AlbumMusicRecord> MusicRecords => _musicRecords;
        public Album(
            string mid, int albumId, string albumName, string albumDescriptions,
            string albumGenre, string albumLanguage, string albumCoverUrl,
            int albumSongCount, string publishCompany, string publishTime,
            List<AlbumMusicRecord> musicRecords,
            List<TagSummary> tagSummaries) : base(tagSummaries)
        {
            Id = mid;
            AlbumId = albumId;
            AlbumName = albumName;
            AlbumDescriptions = albumDescriptions;
            AlbumGenre = albumGenre;
            AlbumLanguage = albumLanguage;
            AlbumCoverUrl = albumCoverUrl;
            AlbumSongCount = albumSongCount > 0 ? albumSongCount : throw new ArgumentException("AlbumSongCount must be at least 1.");
            PublishCompany = publishCompany;
            PublishTime = publishTime;
            _musicRecords = musicRecords;
        }

        protected override IReferrer ToReferrer()
        {
            throw new NotImplementedException();
        }
    }
}
