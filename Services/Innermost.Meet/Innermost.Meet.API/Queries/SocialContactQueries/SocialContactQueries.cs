﻿using Innermost.Meet.API.Queries.StatueQueries;

namespace Innermost.Meet.API.Queries.SocialContactQueries
{
    public class SocialContactQueries : ISocialContactQueries
    {
        private readonly MeetMongoDBContext _context;
        private readonly IUserIdentityService _userIdentityService;
        private readonly IStatueQueries _statueQueries;
        public SocialContactQueries(MeetMongoDBContext context, IUserIdentityService userIdentityService,IStatueQueries statueQueries)
        {
            _context = context;
            _userIdentityService = userIdentityService;
            _statueQueries = statueQueries;

        }
        public async Task<IEnumerable<ConfidantRequestDTO>> GetConfidantRequestsToBeReviewedAsync()
        {
            var userId=_userIdentityService.GetUserId();

            var userSocialContact = await _context.UserSocialContacts.Find(u => u.UserId == userId).FirstAsync();

            var confidantRequests = userSocialContact.ConfidantRequests.Where(cr=>cr.ConfidantRequestStatue==ConfidantRequestStatue.ToBeReviewed).Select(async cr =>
            {
                var requestUserNames=await _userIdentityService.GetUserNamesAsync(cr.RequestUserId);
                var requestUserAvatarUrl=await _userIdentityService.GetUserAvatarUrlAsync(cr.RequestUserId);

                return new ConfidantRequestDTO(cr.Id!, cr.RequestUserId, requestUserNames.userName, requestUserNames.userNickName, requestUserAvatarUrl, cr.RequestMessage, cr.ConfidantRequestStatue.Name, cr.RequestTime, cr.UpdateTime);
            });

            return await Task.WhenAll(confidantRequests);
        }

        public async Task<IEnumerable<ConfidantDTO>> GetConfidantsAsync()
        {
            var userId=_userIdentityService.GetUserId();

            var userSocialContact=await _context.UserSocialContacts.Find(u => u.UserId == userId).FirstAsync();

            var confidants = userSocialContact.Confidants.Select(async c =>
            {
                var confidantUserNames = await _userIdentityService.GetUserNamesAsync(c.Id);
                var confidantUserAvatarUrl = await _userIdentityService.GetUserAvatarUrlAsync(c.Id);
                var confidantStatue = await _statueQueries.GetOneUserStatueAsync(c.Id!);

                return new ConfidantDTO(c.Id!, confidantUserNames.userName, confidantUserNames.userNickName, confidantUserAvatarUrl, confidantStatue.UserStatue, confidantStatue.OnlineStatue);
            });

            return await Task.WhenAll(confidants);
        }
    }
}
