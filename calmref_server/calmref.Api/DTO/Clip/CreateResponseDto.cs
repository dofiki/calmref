namespace calmref_server.DTO.Clip;

public class CreateResponseDto
{
    public string clipId { get; set; }
    public Guid? userId { get; set; }
    public string? title { get; set; } = string.Empty;
    public string? content { get; set; } = string.Empty;
    public bool readOnly { get; set; }
}