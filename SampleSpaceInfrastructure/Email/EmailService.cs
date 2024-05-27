using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Options;
using MimeKit;
using MimeKit.Text;
using SampleSpaceBll.Abstractions.Email;

namespace SampleSpaceInfrastructure.Email;

public class EmailService(IOptions<EmailServiceOptions> options) : IEmailService
{
    private readonly EmailServiceOptions _options = options.Value;
    
    public async Task Send(string to, string subject, string html, string? from = null)
    {
        var email = new MimeMessage();
        email.From.Add(MailboxAddress.Parse(from ?? _options.EmailFrom));
        email.To.Add(MailboxAddress.Parse(to));
        email.Subject = subject;
        email.Body = new TextPart(TextFormat.Html) { Text = html };

        using var smtp = new SmtpClient();
        await smtp.ConnectAsync(_options.SmtpHost, _options.SmtpPort, SecureSocketOptions.StartTls);
        await smtp.AuthenticateAsync(_options.SmtpUser, _options.SmtpPass);
        await smtp.SendAsync(email);
        await smtp.DisconnectAsync(true);
    }
}