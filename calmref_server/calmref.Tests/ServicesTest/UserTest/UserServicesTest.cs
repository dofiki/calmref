using calmref_server.Data;
using calmref_server.DTO.User;
using Microsoft.EntityFrameworkCore;
using Xunit;
using calmref_server.Models;

public class UserServiceTests : IDisposable
{
    private readonly DbContextOptions<CrefDbContext> _options;
    private readonly CrefDbContext _context;
    private readonly UserService _sut;

    public UserServiceTests()
    {
        _options = new DbContextOptionsBuilder<CrefDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        _context = new CrefDbContext(_options);
        _sut = new UserService(_context);
    }

    public void Dispose() => _context.Dispose();


    [Fact]
    public async Task GetUser_ExistingUser_ReturnsDtoWithUsernameAndCreatedAt()
    {
        var userId = Guid.NewGuid();
        var createdAt = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc);
        _context.Users.Add(new User { userId = userId, username = "alice", createdAt = createdAt });
        await _context.SaveChangesAsync();

        var result = await _sut.GetUserAsync(userId);

        Assert.IsType<GetUserDto>(result);
        Assert.Equal("alice", result.username);
        Assert.Equal(createdAt, result.createdAt);
    }


    [Fact]
    public async Task GetUser_UserDoesNotExist_ThrowsNotFoundException()
    {
        var userId = Guid.NewGuid();

        await Assert.ThrowsAsync<NotFoundException>(
            () => _sut.GetUserAsync(userId)
        );
    }


    [Fact]
    public async Task GetUserClips_UserHasClips_ReturnsOnlyThatUsersClips()
    {
        var userId = Guid.NewGuid();
        var created = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc);
        var updated = new DateTime(2026, 2, 1, 0, 0, 0, DateTimeKind.Utc);
        _context.Clips.Add(new Clip
        {
            clipId = Guid.NewGuid().ToString(),
            userId = userId,
            title = "title",
            content = "body",
            createdAt = created,
            updatedAt = updated
        });
        await _context.SaveChangesAsync();

        var result = await _sut.GetUserClipsAsync(userId);

        var clip = Assert.Single(result);
        Assert.Equal("title", clip.title);
        Assert.Equal("body", clip.content);
        Assert.Equal(created, clip.createdAt);
        Assert.Equal(updated, clip.updatedAt);
    }

    [Fact]
    public async Task GetUserClips_UserHasNoClips_ReturnsEmptyList()
    {
        var result = await _sut.GetUserClipsAsync(Guid.NewGuid());

        Assert.Empty(result);
    }
}