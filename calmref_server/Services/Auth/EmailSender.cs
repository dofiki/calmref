using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;

public class EmailSender(IConfiguration config) : IEmailSender
{
    public async Task SendAsync(string to, string subject, string body)
    {
        var user = config["Smtp:User"]!; // email address 

        var message = new MimeMessage();
        message.From.Add(new MailboxAddress("CalmRef", user));
        message.To.Add(MailboxAddress.Parse(to));
        message.Subject = subject;
        message.Body = new TextPart("plain") { Text = body };

        using var client = new SmtpClient();
        await client.ConnectAsync
        (
            config["Smtp:Host"]!,
            config.GetValue<int>("Smtp:Port"),
            SecureSocketOptions.StartTls
        );

        await client.AuthenticateAsync(user, config["Smtp:Password"]!);
        await client.SendAsync(message);
        await client.DisconnectAsync(true);
    }
}