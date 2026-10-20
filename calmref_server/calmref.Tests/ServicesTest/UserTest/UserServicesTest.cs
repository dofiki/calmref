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

    // GetUserAsync

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
    public async Task GetUser_UnknownUser_ThrowsNotFoundException()
    {
        await Assert.ThrowsAsync<NotFoundException>(() => _sut.GetUserAsync(Guid.NewGuid()));
    }

    // GetUserClipsAsync

    [Fact]
    public async Task GetUserClips_UserHasClips_ReturnsOnlyThatUsersClips()
    {
        var userId = Guid.NewGuid();
        var otherUserId = Guid.NewGuid();
        _context.Clips.AddRange(
            new Clip { clipId = Guid.NewGuid().ToString(), userId = userId, title = "mine 1", content = "a" },
            new Clip { clipId = Guid.NewGuid().ToString(), userId = userId, title = "mine 2", content = "b" },
            new Clip { clipId = Guid.NewGuid().ToString(), userId = otherUserId, title = "not mine", content = "c" });
        await _context.SaveChangesAsync();

        var result = await _sut.GetUserClipsAsync(userId);

        Assert.Equal(2, result.Count);
        Assert.DoesNotContain(result, c => c.title == "not mine");
    }

    [Fact]
    public async Task GetUserClips_UserHasNoClips_ReturnsEmptyList()
    {
        var result = await _sut.GetUserClipsAsync(Guid.NewGuid());

        Assert.Empty(result);
    }
}