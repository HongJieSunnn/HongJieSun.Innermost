using Innermost.TagReferrers;
using TagS.Core.Models;
using TagS.Microservices.Client.DomainSeedWork;
using TagS.Microservices.Client.Models;

namespace Innermost.LogLife.Domain.AggregatesModels.LifeRecordAggregate
{
    public class LifeRecord
        : TagableEntity<int, LifeRecord>, IAggregateRoot
    {
        private string _userId;
        public string UserId => _userId;

        public string? Title { get; private set; }
        public string Text { get; private set; }

        /// <summary>
        /// UID from Baidu map api.
        /// </summary>
        private string? _locationUId;
        public string? LocationUId => _locationUId;
        public Location? Location { get; set; }//TODO I do not know whether there will call errors when add LifeRecord with exited location.

        /// <summary>
        /// Mid is the music id get by music api.
        /// </summary>
        private string? _musicRecordMId;
        public string? MusicRecordMId => _musicRecordMId;
        public MusicRecord? MusicRecord { get; set; }

        private readonly List<ImagePath>? _imagePaths;
        public IReadOnlyCollection<ImagePath>? ImagePaths => _imagePaths?.AsReadOnly();

        private bool _isShared;

        public DateTime CreateTime { get; private set; }
        public DateTime? UpdateTime { get; private set; }
        public DateTime? DeleteTime { get; private set; }

        protected LifeRecord() : base(new List<TagSummary<int, LifeRecord>>())
        {
        }

        public LifeRecord(
            string userId,
            string? title,
            string text,
            string? locationUId,
            string? mId,
            DateTime createTime,
            DateTime? updateTime,
            DateTime? deleteTime,
            bool isShared,
            List<ImagePath>? imagePaths,
            List<TagSummary<int, LifeRecord>> tagSummaries
        ) : base(tagSummaries)
        {
            _userId = userId;
            Title = title;
            Text = text;
            _locationUId = locationUId;
            _musicRecordMId = mId;
            CreateTime = createTime;
            UpdateTime = updateTime;
            DeleteTime = deleteTime;
            _imagePaths = imagePaths;
            _isShared = false;
        }

        public void SetDeleted()
        {
            DeleteTime = DateTime.Now;
            AddDomainEvent(new LifeRecordDeletedDomainEvent(Id, UserId));
            foreach (var tagSummary in Tags)
            {
                AddDomainEventForRemovingTag(tagSummary);
            }
        }

        public void SetShared()
        {
            if (!_isShared)
            {
                _isShared = true;
                AddDomainEvent(new LifeRecordSetSharedDomainEvent(
                    Id, UserId, Title, Text,
                    _locationUId, Location?.LocationName, Location?.Province, Location?.City, Location?.District, Location?.Address, Location?.BaiduPOI.Longitude, Location?.BaiduPOI.Latitude,
                    MusicRecord?.Id, MusicRecord?.MusicName, MusicRecord?.Singer, MusicRecord?.Album,
                    ImagePaths?.Select(i => i.Path).ToList(),
                    CreateTime, UpdateTime, DeleteTime,
                    Tags.Select(t => (t.TagId, t.TagName)).ToList()
                    ));
            }
        }

        //TODO UpdateFunctions

        public override IReferrer ToReferrer()
        {
            var referrer = new LifeRecordReferrer(
                Id, UserId, Title, Text,
                LocationUId, Location?.LocationName, Location?.Province, Location?.City, Location?.District, Location?.Address, Location?.BaiduPOI.Longitude, Location?.BaiduPOI.Latitude,
                MusicRecordMId, MusicRecord?.MusicName, MusicRecord?.Singer, MusicRecord?.Album,
                ImagePaths?.Select(i => i.Path).ToList(),
                CreateTime, UpdateTime, DeleteTime
            );

            return referrer;
        }
    }
}
