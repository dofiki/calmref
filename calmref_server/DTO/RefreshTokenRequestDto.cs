namespace DTO;

public class RefreshTokenRequestDto
{
    public Guid userId { get; set; }
    public required string RefreshToken { get; set; }
}