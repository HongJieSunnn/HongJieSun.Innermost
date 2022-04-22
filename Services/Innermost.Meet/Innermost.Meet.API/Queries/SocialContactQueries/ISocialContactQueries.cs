namespace Innermost.Meet.API.Queries.SocialContactQueries
{
    public interface ISocialContactQueries
    {
        Task<IEnumerable<ConfidantRequestDTO>> GetConfidantRequestsToBeReviewedAsync();
        Task<IEnumerable<ConfidantDTO>> GetConfidantsAsync();
    }
}
