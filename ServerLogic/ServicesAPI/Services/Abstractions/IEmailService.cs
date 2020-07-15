using System.Threading.Tasks;

namespace ServicesAPI.Services.Abstractions
{
    public interface IEmailService
    {
        Task SendConfirmMessageAsync(int userId, string token, string email, string protocol);
        void SendNotifyMessage(string email, string courseDetails);
    }
}
