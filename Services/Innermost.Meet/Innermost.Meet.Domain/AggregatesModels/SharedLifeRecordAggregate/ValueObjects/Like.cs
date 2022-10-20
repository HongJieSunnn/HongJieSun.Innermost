namespace Innermost.Meet.Domain.AggregatesModels.SharedLifeRecordAggregate.ValueObjects
{
    public class Like : ValueObject
    {
        public string LikerUserId { get; private set; }
        public string LikerUserName { get; private set; }
        public string LikerUserNickName { get; private set; }
        public string LikerUserAvatarUrl { get; private set; }
        public DateTime LikeTime { get; private set; }
        public Like(string likerUserId, string likerUserName, string likerUserNickName, string likerUserAvatarUrl, DateTime likeTime)
        {
            LikerUserId = likerUserId;
            LikerUserName = likerUserName;
            LikerUserNickName = likerUserNickName;
            LikerUserAvatarUrl = likerUserAvatarUrl;
            LikeTime = likeTime;
        }
        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return LikerUserId;
            yield return LikerUserName;
            yield return LikerUserNickName;
            yield return LikerUserAvatarUrl;
            yield return LikeTime;
        }
    }
}
