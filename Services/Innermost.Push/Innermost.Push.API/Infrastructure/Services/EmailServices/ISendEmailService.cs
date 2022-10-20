using FluentEmail.Core.Models;

namespace Innermost.Push.API.Infrastructure.Services.EmailServices
{
    public interface ISendEmailService
    {
        Task<SendResponse> SendEmailAsync(string toEmail, string subject, string body, bool ishtml = false);
    }
}
