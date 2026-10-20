namespace calmref_server.DTO.Clip;

public class GetClipEditorDto
{
    public string username { get; set; } = string.Empty;
    public DateTime? editedAt { get; set; }
}