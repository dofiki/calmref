using MailKit.Security;
using MimeKit;

public interface ISmtpService
{
    Task ConnectAsync(string host, int port);
    Task AuthenticateAsync(string user, string password);
    Task SendAsync(MimeMessage message);
    Task DisconnectAsync();
}