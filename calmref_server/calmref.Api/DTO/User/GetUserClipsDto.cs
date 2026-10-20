namespace calmref_server.DTO.User;

public class GetUserClipsDto
{
    public string? title { get; set; } = string.Empty;
    public string? content { get; set; } = string.Empty;
    public bool readOnly { get; set; }
    public DateTime createdAt { get; set; }
    public DateTime? updatedAt { get; set; }
}