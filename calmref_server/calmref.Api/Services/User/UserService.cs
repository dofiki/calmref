using calmref_server.Data;
using calmref_server.DTO.User;
using Microsoft.EntityFrameworkCore;

public class UserService(CrefDbContext context) : IUserService
{
    public async Task<GetUserDto> GetUserAsync(Guid userId)
    {
        var user = await context.Users.FirstOrDefaultAsync(u => u.userId == userId);

        if (user is null)
        {
            throw new NotFoundException("User not found !");
        }

        return new GetUserDto
        {
            username = user.username,
            createdAt = user.createdAt
        };
    }

    public async Task<List<GetUserClipsDto>> GetUserClipsAsync(Guid userId)
    {
        var clips = await context.Clips
                .Where(c => c.userId == userId)
                .Select(c => new GetUserClipsDto
                {
                    title = c.title,
                    content = c.content,
                    createdAt = c.createdAt,
                    updatedAt = c.updatedAt
                })
                .ToListAsync();

        return clips;
    }

}