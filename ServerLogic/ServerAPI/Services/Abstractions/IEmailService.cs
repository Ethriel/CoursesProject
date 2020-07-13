using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ServerAPI.Services.Abstractions
{
    public interface IEmailService
    {
        Task SendConfirmMessageAsync(int userId, string token, string email, string protocol);
        Task SendNotifyMessageAsync();
    }
}
