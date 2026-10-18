namespace calmref_server.Models;

using System.ComponentModel.DataAnnotations;

public class User
{
    public required Guid userId { get; set; }

    [Required]
    [MaxLength(255)]
    public string username { get; set; } = string.Empty;

    [Required]
    [MaxLength(255)]
    public string email { get; set; } = string.Empty;
    public bool emailVerified { get; set; }
    public DateTime createdAt { get; set; }

    public EmailOtp? EmailOtp { get; set; }
    public UserCredential? UserCredential { get; set; }
    public List<Clip> Clips { get; set; } = new();
}