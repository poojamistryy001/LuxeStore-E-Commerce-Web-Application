using Microsoft.AspNetCore.Identity.UI.Services;

namespace LuxeStore.Services
{
    public class EmailSender : IEmailSender
    {
        public Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            // Do nothing (dummy)
            return Task.CompletedTask;
        }
    }
}