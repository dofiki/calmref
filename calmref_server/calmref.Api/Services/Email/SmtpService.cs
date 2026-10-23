using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;

public class SmtpService : ISmtpService
{
    private readonly SmtpClient _client = new();

    public Task ConnectAsync(string host, int port)
    {
        return _client.ConnectAsync(
            host, port, SecureSocketOptions.StartTls);
    }

    public Task AuthenticateAsync(string user, string password)
    {
        return _client.AuthenticateAsync(user, password);
    }

    public Task SendAsync(MimeMessage message)
    {
        return _client.SendAsync(message);
    }

    public async Task DisconnectAsync()
    {
        await _client.DisconnectAsync(true);
        _client.Dispose();
    }
}