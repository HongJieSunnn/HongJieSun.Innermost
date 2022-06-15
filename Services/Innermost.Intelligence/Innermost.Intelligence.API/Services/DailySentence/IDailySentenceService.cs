namespace Innermost.Intelligence.API.Services.DailySentence
{
    public interface IDailySentenceService
    {
        Task<string> GetDailySentenceAsync();

        Task<string> GetRandomDateDailySentenceAsync();
    }
}
