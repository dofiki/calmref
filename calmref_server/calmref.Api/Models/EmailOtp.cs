namespace calmref_server.Models;

public class EmailOtp
{
    public Guid EmailOtpId { get; set; }
    public Guid userId { get; set; }
    public string hashedOtp { get; set; } = string.Empty;
    public DateTime OtpExpiresAt { get; set; }
    public int failedAttempts { get; set; }
    public DateTime? LockedUntil { get; set; }
    public User? User { get; set; }
}