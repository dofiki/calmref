using calmref_server.Data;
using calmref_server.DTO.Clip;
using Microsoft.EntityFrameworkCore;
using calmref_server.Models;

public class ClipServiceTest : IDisposable
{

    private readonly DbContextOptions<CrefDbContext> _options;
    private readonly CrefDbContext _context;
    private readonly ClipService _sut;

    public ClipServiceTest()
    {

        _options = new DbContextOptionsBuilder<CrefDbContext>()
        .UseInMemoryDatabase(Guid.NewGuid().ToString())
        .Options;

        _context = new CrefDbContext(_options);
        _sut = new ClipService(_context);

    }

    public void Dispose() => _context.Dispose();

    [Fact]
    public async Task GetClip_ClipExists_ReturnsGetClipDto()
    {

        string slug = "math-notes";
        var userId = Guid.NewGuid();

        var clipId = "math-notes";
        var newClip = new Clip
        {
            clipId = clipId,
            userId = userId,
            title = "clip-title",
            content = "clip-content",
            readOnly = false,
            createdAt = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc)
        };

        await _context.Clips.AddAsync(newClip);
        await _context.SaveChangesAsync();

        var result = await _sut.GetClipAsync(slug, userId);

        Assert.IsType<GetClipDto>(result);
        Assert.Equal(slug, result.clipId);
        Assert.Equal("clip-title", result.title);
        Assert.Equal("clip-content", result.content);
        Assert.False(result.readOnly);
        Assert.Equal(newClip.createdAt, result.createdAt);
        Assert.True(result.isOwner);
    }

    [Fact]
    public async Task GetClip_ClipDoesNotExist_ThrowsNotFoundException()
    {
        var slug = Guid.NewGuid().ToString();
        var userId = Guid.NewGuid();

        await Assert.ThrowsAsync<NotFoundException>(
                () => _sut.GetClipAsync(slug, userId)
            );

    }

    [Fact]
    public async Task CreateClip_ValidRequest_ReturnsCreateResponseDto()
    {
        var slug = "math-notes";
        var userId = Guid.NewGuid();

        var request = new CreateClipDto
        {
            title = "Math Notes",
            content = "Algebra revision",
            readOnly = false
        };

        var result = await _sut.CreateClipAsync(request, userId, slug);

        Assert.IsType<CreateResponseDto>(result);
        Assert.Equal(userId, result.userId);
        Assert.Equal(slug, result.clipId);
        Assert.Equal("Math Notes", result.title);
        Assert.Equal("Algebra revision", result.content);
        Assert.False(result.readOnly);

        var savedClip = await _context.Clips
            .FirstOrDefaultAsync(c => c.clipId == slug);

        Assert.NotNull(savedClip);
        Assert.Equal(userId, savedClip.userId);
    }

    [Fact]
    public async Task CreateClip_SlugAlreadyExists_ThrowsConflictException()
    {
        var slug = "math-notes";

        var existingClip = new Clip
        {
            clipId = slug,
            title = "Existing Clip",
            content = "Existing content",
            readOnly = false,
            createdAt = DateTime.UtcNow
        };

        await _context.Clips.AddAsync(existingClip);
        await _context.SaveChangesAsync();

        var request = new CreateClipDto
        {
            title = "New Clip",
            content = "New content",
            readOnly = false
        };

        var exception = await Assert.ThrowsAsync<ConflictException>(
            () => _sut.CreateClipAsync(request, Guid.NewGuid(), slug)
        );

        Assert.Equal("Clip Already Exists", exception.Message);

        var clips = await _context.Clips
            .Where(c => c.clipId == slug)
            .ToListAsync();

        Assert.Single(clips);
        Assert.Equal("Existing Clip", clips[0].title);
    }

    [Fact]
    public async Task CreateClip_AnonymousUser_ReadOnlyIsFalse()
    {
        var slug = "public-notes";

        var request = new CreateClipDto
        {
            title = "Public Notes",
            content = "Public content",
            readOnly = true
        };

        var result = await _sut.CreateClipAsync(request, null, slug);

        Assert.Null(result.userId);
        Assert.False(result.readOnly);
    }

    [Fact]
    public async Task CreateClip_AuthenticatedUserRequestsReadOnly_ReadOnlyIsTrue()
    {
        var slug = "private-notes";
        var userId = Guid.NewGuid();

        var request = new CreateClipDto
        {
            title = "Private Notes",
            content = "Some content",
            readOnly = true
        };

        var result = await _sut.CreateClipAsync(request, userId, slug);

        Assert.Equal(userId, result.userId);
        Assert.True(result.readOnly);
    }

    [Fact]
    public async Task UpdateClip_ClipExists_ReturnsUpdatedClip()
    {
        var slug = "math-notes";
        var userId = Guid.NewGuid();

        var clip = new Clip
        {
            clipId = slug,
            userId = userId,
            title = "Old title",
            content = "Old content",
            readOnly = false,
            createdAt = DateTime.UtcNow
        };

        await _context.Clips.AddAsync(clip);
        await _context.SaveChangesAsync();

        var request = new UpdateClipDto
        {
            title = "Updated title",
            content = "Updated content",
            readOnly = true
        };

        var result = await _sut.UpdateClipAsync(request, userId, slug);

        Assert.Equal("Updated title", result.title);
        Assert.Equal("Updated content", result.content);
        Assert.True(result.readOnly);
        Assert.NotEqual(default, result.updatedAt);

        await using var verifyContext = new CrefDbContext(_options);

        var updatedClip = await verifyContext.Clips
            .FirstOrDefaultAsync(c => c.clipId == slug);

        Assert.NotNull(updatedClip);
        Assert.Equal("Updated title", updatedClip.title);
        Assert.Equal("Updated content", updatedClip.content);
        Assert.True(updatedClip.readOnly);

        var editorCount = await _context.ClipEditors
            .CountAsync(e => e.clipId == slug);

        Assert.Equal(1, editorCount);
    }

    [Fact]
    public async Task UpdateClip_ClipDoesNotExist_ThrowsNotFoundException()
    {

        var slug = "missing-clip";
        var userId = Guid.NewGuid();

        var request = new UpdateClipDto
        {
            title = "Updated title",
            content = "Updated content",
            readOnly = false
        };

        await Assert.ThrowsAsync<NotFoundException>(
            () => _sut.UpdateClipAsync(request, userId, slug)
        );
    }

    [Fact]
    public async Task UpdateClip_ReadOnlyClipAndNonOwner_ThrowsBadRequestException()
    {
        var slug = "readonly-clip";
        var ownerId = Guid.NewGuid();
        var otherUserId = Guid.NewGuid();

        var clip = new Clip
        {
            clipId = slug,
            userId = ownerId,
            title = "Original title",
            content = "Original content",
            readOnly = true,
            createdAt = DateTime.UtcNow
        };

        await _context.Clips.AddAsync(clip);
        await _context.SaveChangesAsync();

        var request = new UpdateClipDto
        {
            title = "Changed title",
            content = "Changed content",
            readOnly = false
        };

        var exception = await Assert.ThrowsAsync<BadRequestException>(
            () => _sut.UpdateClipAsync(request, otherUserId, slug)
        );

        Assert.Equal("This clip is read only", exception.Message);

        var unchangedClip = await _context.Clips
            .FirstAsync(c => c.clipId == slug);

        Assert.Equal("Original title", unchangedClip.title);
        Assert.Equal("Original content", unchangedClip.content);
        Assert.True(unchangedClip.readOnly);

        Assert.Empty(await _context.ClipEditors
            .Where(e => e.clipId == slug)
            .ToListAsync());
    }

    [Fact]
    public async Task UpdateClip_OwnerUpdatesReadOnlySetting_UpdatesSuccessfully()
    {
        var slug = "owner-clip";
        var ownerId = Guid.NewGuid();

        var clip = new Clip
        {
            clipId = slug,
            userId = ownerId,
            title = "Original title",
            content = "Original content",
            readOnly = false,
            createdAt = DateTime.UtcNow
        };

        await _context.Clips.AddAsync(clip);
        await _context.SaveChangesAsync();

        var request = new UpdateClipDto
        {
            title = "New title",
            content = "New content",
            readOnly = true
        };

        var result = await _sut.UpdateClipAsync(request, ownerId, slug);

        Assert.Equal("New title", result.title);
        Assert.Equal("New content", result.content);
        Assert.True(result.readOnly);

        // to check
        await using var verifyContext = new CrefDbContext(_options);

        var updatedClip = await verifyContext.Clips
            .FirstOrDefaultAsync(c => c.clipId == slug);

        Assert.True(updatedClip.readOnly);
    }

    [Fact]
    public async Task UpdateClip_NonOwnerUpdatesWritableClip_ReadOnlySettingRemainsUnchanged()
    {
        var slug = "shared-clip";
        var ownerId = Guid.NewGuid();
        var editorId = Guid.NewGuid();

        var clip = new Clip
        {
            clipId = slug,
            userId = ownerId,
            title = "Original title",
            content = "Original content",
            readOnly = false,
            createdAt = DateTime.UtcNow
        };

        await _context.Clips.AddAsync(clip);
        await _context.SaveChangesAsync();

        var request = new UpdateClipDto
        {
            title = "Edited title",
            content = "Edited content",
            readOnly = true
        };

        var result = await _sut.UpdateClipAsync(request, editorId, slug);

        Assert.Equal("Edited title", result.title);
        Assert.Equal("Edited content", result.content);

        Assert.False(result.readOnly);

        await using var verifyContext = new CrefDbContext(_options);

        var updatedClip = await verifyContext.Clips
            .FirstOrDefaultAsync(c => c.clipId == slug);

        Assert.False(updatedClip.readOnly);
    }

    [Fact]
    public async Task UpdateClip_SuccessfulEdit_RecordsEditor()
    {
        var slug = "tracked-clip";
        var ownerId = Guid.NewGuid();

        var clip = new Clip
        {
            clipId = slug,
            userId = ownerId,
            title = "Original title",
            content = "Original content",
            readOnly = false,
            createdAt = DateTime.UtcNow
        };

        await _context.Clips.AddAsync(clip);
        await _context.SaveChangesAsync();

        var request = new UpdateClipDto
        {
            title = "Edited title",
            content = "Edited content",
            readOnly = false
        };

        await _sut.UpdateClipAsync(request, ownerId, slug);

        var editorEntry = await _context.ClipEditors
            .FirstOrDefaultAsync(e => e.clipId == slug);

        Assert.NotNull(editorEntry);
        Assert.Equal(ownerId, editorEntry.userId);
        Assert.NotEqual(Guid.Empty, editorEntry.clipEditorId);
    }

    [Fact]
    public async Task GetClipEditors_ClipHasEditors_ReturnsEditorDtos()
    {
        var slug = "math-notes";
        var userId = Guid.NewGuid();
        var editedAt = new DateTime(
            2026, 1, 1, 10, 0, 0, DateTimeKind.Utc);

        var user = new User
        {
            userId = userId,
            username = "john"
        };

        await _context.Users.AddAsync(user);

        await _context.ClipEditors.AddAsync(new ClipEditor
        {
            clipEditorId = Guid.NewGuid(),
            clipId = slug,
            userId = userId,
            editedAt = editedAt
        });

        await _context.SaveChangesAsync();

        var result = await _sut.GetClipEditorsAsync(slug);

        var editor = Assert.Single(result);
        Assert.Equal("john", editor.username);
        Assert.Equal(editedAt, editor.editedAt);
    }

    [Fact]
    public async Task GetClipEditors_NoEditors_ReturnsEmptyList()
    {
        var slug = "math-notes";

        var result = await _sut.GetClipEditorsAsync(slug);

        Assert.NotNull(result);
        Assert.Empty(result);
    }

    [Fact]
    public async Task GetClipEditors_AnonymousEditor_ReturnsAnonymous()
    {
        var slug = "math-notes";
        var editedAt = DateTime.UtcNow;

        await _context.ClipEditors.AddAsync(new ClipEditor
        {
            clipEditorId = Guid.NewGuid(),
            clipId = slug,
            userId = null,
            editedAt = editedAt
        });

        await _context.SaveChangesAsync();

        var result = await _sut.GetClipEditorsAsync(slug);

        var editor = Assert.Single(result);
        Assert.Equal("Anonymous", editor.username);
        Assert.Equal(editedAt, editor.editedAt);
    }

}