namespace Innermost.Meet.Domain.AggregatesModels.SharedLifeRecord.ValueObjects
{
    public class Like : ValueObject
    {
        public string LikeUserId { get;private set; }
        public string LikeUserName { get; set; }
        public string LikeUserAvatarUrl { get; private set; }
        public DateTime LikeTime { get;private set; }
        public Like(string likeUserId,string likeUserName,string likeUserAvatarUrl,DateTime likeTime)
        {
            LikeUserId= likeUserId;
            LikeUserName= likeUserName;
            LikeUserAvatarUrl= likeUserAvatarUrl;
            LikeTime= likeTime;
        }
        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return LikeUserId;
            yield return LikeUserName;
            yield return LikeUserAvatarUrl;
            yield return LikeTime;
        }
    }
}
