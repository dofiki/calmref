using System.Security.Claims;
using calmref_server.DTO.Clip;
using Microsoft.AspNetCore.Mvc;

[Route("api/[controller]")]
[ApiController]
public class ClipController(IClipService clipService) : ControllerBase
{

    [HttpGet("{slug}")]
    public async Task<ActionResult<GetClipDto>> GetClip(string slug)
    {
        var trimmedSlug = TrimSlug(slug);
        var userId = GetUserId();

        var clip = await clipService.GetClipAsync(trimmedSlug, userId);

        return clip;
    }

    [HttpPost("{slug}")]
    public async Task<ActionResult<CreateClipDto>> CreateClip(string slug,
    [FromBody] CreateClipDto request)
    {
        var trimmedSlug = TrimSlug(slug);
        var userId = GetUserId();

        var clip = await clipService.CreateClipAsync(request, userId, trimmedSlug);

        return Ok(clip);

    }

    [HttpPut("{slug}")]
    public async Task<ActionResult<CreateClipDto>> UpdateClip(string slug,
    [FromBody] UpdateClipDto request)
    {
        var trimmedSlug = TrimSlug(slug);
        var userId = GetUserId();

        var clip = await clipService.UpdateClipAsync(request, userId, trimmedSlug);

        return Ok(clip);

    }

    [HttpGet("{slug}/editors")]
    public async Task<ActionResult<List<GetClipEditorDto>>> GetClipEditors(string slug)
    {
        var trimmedSlug = TrimSlug(slug);
        var editors = await clipService.GetClipEditorsAsync(trimmedSlug);
        return Ok(editors);
    }

    public Guid? GetUserId()
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        Guid? userGuid = Guid.TryParse(userId, out var parsedGuid) ? parsedGuid : null;

        return userGuid;
    }

    public string TrimSlug(string slug)
    {
        return slug.Trim().Replace(" ", "-").ToLowerInvariant();
    }
}