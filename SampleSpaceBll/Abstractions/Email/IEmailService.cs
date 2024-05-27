namespace SampleSpaceBll.Abstractions.Email;

public interface IEmailService
{
    public Task Send(string to, string subject, string html, string? from = null);
}