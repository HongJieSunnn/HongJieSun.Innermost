namespace Innermost.Meet.API.Queries.InteractionQueries
{
    public interface IInteractionQueries
    {
        Task<IEnumerable<RecordLikeDTO>> GetRecordLikesAsync(string userId);
    }
}
