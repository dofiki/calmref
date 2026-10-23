using MailKit.Security;
using MimeKit;

public class EmailSender(IConfiguration config, ISmtpService smtp) : IEmailSender
{
    public async Task SendAsync(string to, string subject, string body)
    {
        var user = config["Smtp:User"]!;

        var message = new MimeMessage();
        message.From.Add(new MailboxAddress("CalmRef", user));
        message.To.Add(MailboxAddress.Parse(to));
        message.Subject = subject;
        message.Body = new TextPart("plain") { Text = body };

        await smtp.ConnectAsync
        (
            config["Smtp:Host"]!,
            config.GetValue<int>("Smtp:Port")
        );

        await smtp.AuthenticateAsync(user, config["Smtp:Password"]!);
        await smtp.SendAsync(message);
        await smtp.DisconnectAsync();
    }
}