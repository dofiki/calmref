namespace calmref_server.Models;

public class UserCredential
{
    public Guid UserCredentialId { get; set; }
    public Guid userId { get; set; }
    public string hashedPassword { get; set; } = string.Empty;
    public string hashedRefreshToken { get; set; } = string.Empty;
    public DateTime RefreshTokenExpiresAt { get; set; }
    public User? User { get; set; }
}