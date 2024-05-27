using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;
using MimeKit.Text;
using SampleSpaceBll.Abstractions.Email;

namespace SampleSpaceInfrastructure.Email;

public class EmailService(EmailServiceOptions options) : IEmailService
{
    public async Task Send(string to, string subject, string html, string? from = null)
    {
        var email = new MimeMessage();
        email.From.Add(MailboxAddress.Parse(from ?? options.EmailFrom));
        email.To.Add(MailboxAddress.Parse(to));
        email.Subject = subject;
        email.Body = new TextPart(TextFormat.Html) { Text = html };

        using var smtp = new SmtpClient();
        await smtp.ConnectAsync(options.SmtpHost, options.SmtpPort, SecureSocketOptions.StartTls);
        await smtp.AuthenticateAsync(options.SmtpUser, options.SmtpPass);
        await smtp.SendAsync(email);
        await smtp.DisconnectAsync(true);
    }
}