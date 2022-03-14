using TagS.Microservices.Client.DomainSeedWork;
using TagS.Microservices.Client.Models;

namespace Innermost.LogLife.Domain.AggregatesModels.LifeRecordAggregate
{
    public class LifeRecord
        : TagableEntity<int,LifeRecord>, IAggregateRoot
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
        private string? _mId;
        public string? MId => _mId;
        public MusicRecord? MusicRecord { get; set; }

        public List<ImagePath>? ImagePaths { get; set; }

        private bool _isShared;

        public DateTime CreateTime { get; private set; }
        public DateTime? UpdateTime { get; private set; }
        public DateTime? DeleteTime { get; private set; }

        protected LifeRecord():base(new List<TagSummary<int, LifeRecord>>())
        {
        }

        public LifeRecord(
            string userId,
            string? title,
            string text,
            string? locationUId,
            string? mId,
            DateTime publishTime,
            DateTime? updateTime,
            DateTime? deleteTime,
            bool isShared,
            List<ImagePath>? imagePaths,
            List<TagSummary<int,LifeRecord>> tagSummaries
        ):base(tagSummaries)
        {
            _userId=userId; 
            Title=title;
            Text=text;
            _locationUId=locationUId;
            _mId=mId;
            CreateTime=publishTime;
            UpdateTime = updateTime;
            DeleteTime = deleteTime;
            ImagePaths=imagePaths;
            if(isShared)
            {
                SetShared();
            }
        }

        public void SetDeleted()
        {
            DeleteTime = DateTime.Now;
            AddDomainEvent(new LifeRecordDeletedDomainEvent(Id,UserId));
        }

        public void SetShared()
        {
            if (!_isShared)
            {
                _isShared = true;
                AddDomainEvent(new LifeRecordSetSharedDomainEvent(
                    Id,UserId,Title,Text,
                    _locationUId,Location?.Name,Location?.Province,Location?.City,Location?.District,Location?.Address,Location?.BaiduPOI.Longitude,Location?.BaiduPOI.Latitude,
                    MusicRecord?.Id,MusicRecord?.MusicName,MusicRecord?.Singer,MusicRecord?.Album,
                    ImagePaths?.Select(i=>i.Path).ToList(),
                    CreateTime,UpdateTime,DeleteTime,
                    Tags.Select(t=>(t.TagId,t.Name)).ToList()
                    ));
            }
        }

        //TODO UpdateFunctions

        protected override IReferrer ToReferrer()
        {
            throw new NotImplementedException();//TODO
        }
    }
}
