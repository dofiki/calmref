namespace calmref_server.DTO.Auth;

using System.ComponentModel.DataAnnotations;

public class VerifyOtpDto
{
    [Required]
    public Guid userId { get; set; }

    [Required]
    [MinLength(6), MaxLength(6)]
    public string otp { get; set; } = string.Empty;
}