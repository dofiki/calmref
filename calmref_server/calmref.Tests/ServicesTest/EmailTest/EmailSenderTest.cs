using Microsoft.Extensions.Configuration;
using MimeKit;
using Moq;
public class EmailSenderTest
{
    [Fact]
    public async Task SendAsync_SendsCorrectEmail_()
    {
        var settings = new Dictionary<string, string?>
        {
            ["Smtp:Host"] = "localhost",
            ["Smtp:Port"] = "587",
            ["Smtp:User"] = "sender@test.com",
            ["Smtp:Password"] = "password"
        };

        var config = new ConfigurationBuilder().AddInMemoryCollection(settings).Build();
        var smtpMock = new Mock<ISmtpService>();

        MimeMessage? capturedMessage = null;

        smtpMock.Setup(x => x.SendAsync(It.IsAny<MimeMessage>()))
                .Callback<MimeMessage>(
                    message => capturedMessage = message)
                .Returns(Task.CompletedTask);

        var sender = new EmailSender(config, smtpMock.Object);

        await sender.SendAsync(
             "recipient@test.com",
             "Welcome",
             "Welcome to CalmRef!");

        Assert.NotNull(capturedMessage);
        Assert.Equal("Welcome", capturedMessage.Subject);
        Assert.Equal(
            "recipient@test.com",
            capturedMessage.To.Mailboxes.Single().Address);
        Assert.Equal(
            "Welcome to CalmRef!",
            ((TextPart)capturedMessage.Body).Text);

        smtpMock.Verify(
        x => x.ConnectAsync("localhost", 587),
        Times.Once);

        smtpMock.Verify(
            x => x.AuthenticateAsync("sender@test.com", "password"),
            Times.Once);

        smtpMock.Verify(
            x => x.SendAsync(It.IsAny<MimeMessage>()),
            Times.Once);

        smtpMock.Verify(
            x => x.DisconnectAsync(),
            Times.Once);
    }
}