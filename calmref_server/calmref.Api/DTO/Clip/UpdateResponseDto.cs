namespace calmref_server.DTO.Clip;

public class UpdateResponseDto
{

    public string? title { get; set; } = string.Empty;
    public string? content { get; set; } = string.Empty;
    public bool readOnly { get; set; }
    public DateTime updatedAt { get; set; }

}