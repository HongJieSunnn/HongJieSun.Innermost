namespace Innermost.Meet.API.Queries.InteractionQueries
{
    public class InteractionQueries : IInteractionQueries
    {
        private readonly MeetMongoDBContext _context;
        public InteractionQueries(MeetMongoDBContext context)
        {
            _context = context;

        }

        public async Task<IEnumerable<RecordLikeDTO>> GetRecordLikesAsync(string userId)
        {
            var userInteraction = await _context.UserInteractions.Find(ui => ui.UserId == userId).FirstAsync();

            return userInteraction.RecordLikes.Select(rl => MapToRecordLikeDTO(rl));
        }

        private RecordLikeDTO MapToRecordLikeDTO(RecordLike recordLike)
        {
            return new RecordLikeDTO(
                recordLike.Id!,
                recordLike.RecordUserId, recordLike.RecordUserName, recordLike.RecordUserNickName, recordLike.RecordUserAvatarUrl,
                recordLike.RecordTitle, recordLike.RecordText,
                recordLike.RecordMusicRecordName,
                recordLike.RecordLocation,
                recordLike.RecordCreateTime, recordLike.LikeTime);
        }
    }
}
