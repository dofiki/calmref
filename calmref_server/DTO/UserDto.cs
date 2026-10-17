namespace DTO;

using System.ComponentModel.DataAnnotations;
public class UserDto
{
    [Required]
    [EmailAddress]
    public string email { get; set; } = string.Empty;

    [Required]
    [MinLength(8)]
    public string password { get; set; } = string.Empty;
}