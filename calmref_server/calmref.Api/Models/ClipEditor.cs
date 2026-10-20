using calmref_server.Models;

public class ClipEditor
{
    public Guid clipEditorId { get; set; }
    public required string clipId { get; set; }
    public Clip? Clip { get; set; }
    public Guid? userId { get; set; }
    public User? User { get; set; }
    public DateTime? editedAt { get; set; }

}