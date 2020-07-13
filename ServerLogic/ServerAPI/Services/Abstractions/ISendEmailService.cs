using System.Threading.Tasks;

namespace ServerAPI.Services.Abstractions
{
    public interface ISendEmailService
    {
        public Task SendEmailAsync(string email, string subject, string message);
    }
}
