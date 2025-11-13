using System.Net;
using System.Net.Mail;
using Microsoft.Extensions.Options;
using UzTube.Application.Common.Email;

namespace UzTube.Application.Services.Impl;

public class EmailService(IOptions<SmtpSettings> smtpConfigs) : IEmailService
{
    private readonly SmtpSettings _smtpConfigs = smtpConfigs.Value;

    public async Task<bool> SendEmailAsync(string email, string subject, string message)
    {
        SmtpClient client = new SmtpClient(_smtpConfigs.Server, _smtpConfigs.Port)
        {
            EnableSsl = _smtpConfigs.UseSsl,
            UseDefaultCredentials = false,
            Credentials = new NetworkCredential(_smtpConfigs.Username, _smtpConfigs.Password)
        };

        MailMessage mailMessage = new MailMessage
        {
            From = new MailAddress(_smtpConfigs.SenderEmail, email),
            Subject = subject,
            Body = message,
            IsBodyHtml = true,
            To = { email }
        };

        await client.SendMailAsync(mailMessage);
        
        return true;
    }
}