namespace calmref_server.DTO.Clip;

public class GetClipDto
{

    public bool isOwner { get; set; }
    public string clipId { get; set; } = string.Empty;
    public string? title { get; set; } = string.Empty;
    public string? content { get; set; } = string.Empty;
    public bool? readOnly { get; set; }
    public DateTime createdAt { get; set; }
    public DateTime? updatedAt { get; set; }

}