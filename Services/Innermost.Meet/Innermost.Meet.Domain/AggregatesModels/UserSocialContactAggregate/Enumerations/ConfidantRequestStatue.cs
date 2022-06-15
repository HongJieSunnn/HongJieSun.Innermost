namespace Innermost.Meet.Domain.AggregatesModels.UserSocialContactAggregate.Enumerations
{
    public class ConfidantRequestStatue : Enumeration
    {
        public static ConfidantRequestStatue ToBeReviewed = new ConfidantRequestStatue(1, "ToBeReviewed");
        public static ConfidantRequestStatue Passed = new ConfidantRequestStatue(2, "Passed");
        public static ConfidantRequestStatue Refused = new ConfidantRequestStatue(3, "Refused");
        public static ConfidantRequestStatue RefusedAndNotReceiveRequestAnyMore = new ConfidantRequestStatue(4, "RefusedAndNotReceiveRequestAnyMore");
        public ConfidantRequestStatue(int id, string name) : base(id, name)
        {
        }
    }
}
