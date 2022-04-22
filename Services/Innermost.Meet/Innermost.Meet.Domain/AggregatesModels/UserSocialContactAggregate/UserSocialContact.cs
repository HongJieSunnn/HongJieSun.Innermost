using Innermost.Meet.Domain.AggregatesModels.UserSocialContactAggregate.Entities;
using Innermost.Meet.Domain.Events.UserSocialContactEvents;

namespace Innermost.Meet.Domain.AggregatesModels.UserConfidantAggregate
{
    public class UserSocialContact : Entity<string>, IAggregateRoot
    {
        public string UserId { get; init; }

        [BsonRequired]
        [BsonElement("ConfidantRequests")]
        private List<ConfidantRequest> _confidantRequests;
        public IReadOnlyCollection<ConfidantRequest> ConfidantRequests => _confidantRequests.AsReadOnly();

        [BsonRequired]
        [BsonElement("Confidants")]
        private List<Confidant> _confidants;
        public IReadOnlyCollection<Confidant> Confidants => _confidants.AsReadOnly();

        public UserSocialContact(string userId, List<ConfidantRequest> confidantRequests, List<Confidant> confidants)
        {
            UserId = userId;
            _confidantRequests = confidantRequests;
            _confidants = confidants;
        }

        public UpdateDefinition<UserSocialContact> AddConfidant(Confidant confidant)
        {
            _confidants.Add(confidant);

            AddDomainEvent(new AddConfidantDomainEvent(this.Id!, confidant.Id!, confidant.ChattingContextId));

            return Builders<UserSocialContact>.Update.AddToSet("Confidants", confidant);
        }

        public UpdateDefinition<UserSocialContact> AddConfidantRequest(ConfidantRequest confidantRequest)
        {
            _confidantRequests.Add(confidantRequest);
            return Builders<UserSocialContact>.Update.AddToSet("ConfidantRequests", confidantRequest);
        }

        public (FilterDefinition<UserSocialContact>? updateElementFilter, UpdateDefinition<UserSocialContact>? updateDefinition) SetConfidantRequestPassed(string confidantRequestId)
        {
            var confidantRequestToUpdate = _confidantRequests.FirstOrDefault(cr => cr.Id == confidantRequestId && cr.ConfidantRequestStatue == UserSocialContactAggregate.Enumerations.ConfidantRequestStatue.ToBeReviewed);

            if (confidantRequestToUpdate is null)
                return (null, null);

            confidantRequestToUpdate.SetConfidantRequestPassed();

            var updateDefinition = Builders<UserSocialContact>.Update.Set(u => u._confidantRequests[-1].ConfidantRequestStatue, confidantRequestToUpdate.ConfidantRequestStatue);
            var updateElementFilter = Builders<UserSocialContact>.Filter.ElemMatch("ConfidantRequests", Builders<ConfidantRequest>.Filter.Eq(cr => cr.Id, confidantRequestId));

            return (updateElementFilter, updateDefinition);
        }

        public (FilterDefinition<UserSocialContact>? updateElementFilter, UpdateDefinition<UserSocialContact>? updateDefinition) SetConfidantRequestRefused(string confidantRequestId)
        {
            var confidantRequestToUpdate = _confidantRequests.FirstOrDefault(cr => cr.Id == confidantRequestId && cr.ConfidantRequestStatue == UserSocialContactAggregate.Enumerations.ConfidantRequestStatue.ToBeReviewed);

            if (confidantRequestToUpdate is null)
                return (null, null);

            confidantRequestToUpdate.SetConfidantRequestRefused();

            var updateDefinition = Builders<UserSocialContact>.Update.Set(u => u._confidantRequests[-1].ConfidantRequestStatue, confidantRequestToUpdate.ConfidantRequestStatue);
            var updateElementFilter = Builders<UserSocialContact>.Filter.ElemMatch("ConfidantRequests", Builders<ConfidantRequest>.Filter.Eq(cr => cr.Id, confidantRequestId));

            return (updateElementFilter, updateDefinition);
        }

        public (FilterDefinition<UserSocialContact>? updateElementFilter, UpdateDefinition<UserSocialContact>? updateDefinition) SetConfidantRequestRefusedAndNotReceiveRequestAnyMore(string confidantRequestId)
        {
            var confidantRequestToUpdate = _confidantRequests.FirstOrDefault(cr => cr.Id == confidantRequestId && cr.ConfidantRequestStatue == UserSocialContactAggregate.Enumerations.ConfidantRequestStatue.ToBeReviewed);

            if (confidantRequestToUpdate is null)
                return (null, null);

            confidantRequestToUpdate.SetConfidantRequestRefusedAndNotReceiveRequestAnyMore();

            var updateDefinition = Builders<UserSocialContact>.Update.Set(u => u._confidantRequests[-1].ConfidantRequestStatue, confidantRequestToUpdate.ConfidantRequestStatue);
            var updateElementFilter = Builders<UserSocialContact>.Filter.ElemMatch("ConfidantRequests", Builders<ConfidantRequest>.Filter.Eq(cr => cr.Id, confidantRequestId));

            //TODO push refused message?

            return (updateElementFilter, updateDefinition);
        }
    }
}
