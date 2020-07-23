using System.Threading.Tasks;

namespace ServicesAPI.Services.Abstractions
{
    public interface IEmailService
    {
        Task SendConfirmMessageAsync(string token, string email);
        Task SendConfirmChangeEmailAsync(string token, string email);
        void SendNotifyMessage(string email, string courseDetails);
    }
}
