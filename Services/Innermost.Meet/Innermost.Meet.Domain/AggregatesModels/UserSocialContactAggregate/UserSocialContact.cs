using Innermost.Meet.Domain.AggregatesModels.UserSocialContactAggregate.Entities;

namespace Innermost.Meet.Domain.AggregatesModels.UserConfidantAggregate
{
    public class UserSocialContact:Entity<string>,IAggregateRoot
    {
        public string UserId { get; init; }

        private List<ConfidantRequest> _confidantRequests;
        public IReadOnlyCollection<ConfidantRequest> ConfidantRequests => _confidantRequests.AsReadOnly();


        private List<Confidant> _confidants;
        public IReadOnlyCollection<Confidant> Confidants => _confidants.AsReadOnly();

        public UserSocialContact(string userId,List<ConfidantRequest> confidantRequests,List<Confidant> confidants)
        {
            UserId=userId;
            _confidantRequests= confidantRequests;
            _confidants=confidants;
        }

        public void AddConfidant(Confidant confidant)
        {
            _confidants.Add(confidant);
        }
    }
}
