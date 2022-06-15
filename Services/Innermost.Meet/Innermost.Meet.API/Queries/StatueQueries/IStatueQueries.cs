namespace Innermost.Meet.API.Queries.StatueQueries
{
    public interface IStatueQueries
    {
        Task<StatueDTO> GetOneUserStatueAsync(string userId);
        Task<IEnumerable<StatueDTO>> GetManyUserStatuesAsync(IEnumerable<string> userIds);
    }
}
