namespace calmref_server.DTO.Clip;

public class CreateClipDto
{
    public string? title { get; set; } = string.Empty;
    public string? content { get; set; } = string.Empty;
    public bool readOnly { get; set; }
}