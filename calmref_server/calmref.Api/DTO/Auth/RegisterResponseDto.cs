namespace calmref_server.DTO.Auth;

public class RegisterResponseDto
{
    public Guid userId { get; set; }
    public string message { get; set; } = string.Empty;
}