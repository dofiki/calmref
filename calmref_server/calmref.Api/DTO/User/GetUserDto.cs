namespace calmref_server.DTO.User;

public class GetUserDto
{
    public string username { get; set; } = string.Empty;
    public DateTime createdAt { get; set; }
}