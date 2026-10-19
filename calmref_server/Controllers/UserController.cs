using calmref_server.DTO.User;
using Microsoft.AspNetCore.Mvc;

[Route("api/[controller]")]
[ApiController]
public class UserController(IUserService userService) : ControllerBase
{
    [HttpGet("{slug}")]
    public async Task<ActionResult<GetUserDto>> GetUser(string slug)
    {
        if (!Guid.TryParse(slug, out var userId))
        {
            throw new NotFoundException("User not found");
        }

        var user = await userService.GetUserAsync(userId);

        if (user is null)
            throw new NotFoundException("User not found");

        return Ok(user);
    }

    [HttpGet("{slug}/history")]
    public async Task<ActionResult<List<GetUserClipsDto>>> GetUserClips(string slug)
    {
        if (!Guid.TryParse(slug, out var userId))
        {
            throw new NotFoundException("User not found");
        }

        var clips = await userService.GetUserClipsAsync(userId);


        return Ok(clips);
    }

}