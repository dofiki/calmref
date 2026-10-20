namespace calmref_server.DTO.Auth;

public class RefreshTokenRequestDto
{
    public Guid userId { get; set; }
    public required string RefreshToken { get; set; }
}