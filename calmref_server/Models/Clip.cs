namespace calmref_server.Models;

public class Clip
{
    public required string clipId { get; set; }
    public Guid? userId { get; set; }
    public User? User { get; set; }
    public string? title { get; set; } = string.Empty;
    public string? content { get; set; } = string.Empty;
    public bool readOnly { get; set; } = false;
    public DateTime createdAt { get; set; }
    public DateTime? updatedAt { get; set; }

}