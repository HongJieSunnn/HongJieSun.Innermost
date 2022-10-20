using FluentEmail.Core;
using FluentEmail.Core.Models;

namespace Innermost.Push.API.Infrastructure.Services.EmailServices
{
    public class SendEmailService : ISendEmailService
    {
        private readonly IFluentEmail _fluentEmail;
        public SendEmailService(IFluentEmail fluentEmail)
        {
            _fluentEmail = fluentEmail;
        }

        public Task<SendResponse> SendEmailAsync(string toEmail, string subject, string body, bool ishtml = false)
        {
            return _fluentEmail.To(toEmail).Subject(subject).Body(body, ishtml).SendAsync();
        }
    }
}
