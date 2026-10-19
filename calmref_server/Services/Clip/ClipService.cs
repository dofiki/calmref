using calmref_server.DTO.Clip;
using calmref_server.Data;
using calmref_server.Models;
using Microsoft.EntityFrameworkCore;
public class ClipService(CrefDbContext context) : IClipService
{
    public async Task<GetClipDto> GetClipAsync(string slug, Guid? userId)
    {
        var clip = await context.Clips
        .AsNoTracking()
        .FirstOrDefaultAsync(c => c.clipId == slug);

        if (clip is null) throw new NotFoundException("Clip not found!");

        return new GetClipDto
        {
            isOwner = clip.userId is not null && clip.userId == userId,
            clipId = clip.clipId,
            title = clip.title,
            content = clip.content,
            readOnly = clip.readOnly,
            createdAt = clip.createdAt
        };
    }
    public async Task<CreateResponseDto> CreateClipAsync(CreateClipDto request, Guid? userId, string slug)
    {
        var existingClip = await context.Clips.FirstOrDefaultAsync(c => c.clipId == slug);

        if (existingClip is not null)
        {
            throw new ConflictException("Clip Already Exists");
        }

        var now = DateTime.UtcNow;
        var clip = new Clip()
        {
            clipId = slug,
            title = request.title,
            content = request.content,
            // if first part is true we accept the request 
            // otherwise it is false
            readOnly = userId is not null && request.readOnly,
            createdAt = now,
            userId = userId
        };

        context.Add(clip);
        await context.SaveChangesAsync();

        return new CreateResponseDto
        {
            userId = userId,
            clipId = clip.clipId,
            title = clip.title,
            content = clip.content,
            readOnly = clip.readOnly
        };
    }
    public async Task<UpdateResponseDto> UpdateClipAsync(UpdateClipDto request, Guid? userId, string slug)
    {
        var clip = await context.Clips
                    .FirstOrDefaultAsync(c => c.clipId == slug);

        var clipEditor = await context.ClipEditors
                    .FirstOrDefaultAsync(e => e.clipId == slug);

        var editor = await context.Users.AsNoTracking().FirstOrDefaultAsync(u => u.userId == userId);

        if (clip is null) throw new NotFoundException("Clip not found!");

        var isOwner = clip.userId is not null && clip.userId == userId;

        if (clip.readOnly == true && !isOwner)
            throw new BadRequestException("This clip is read only");

        var now = DateTime.UtcNow;

        clip.title = request.title;
        clip.content = request.content;
        clip.readOnly = isOwner ? request.readOnly : clip.readOnly;
        clip.updatedAt = now;

        context.ClipEditors.Add(new ClipEditor
        {
            clipEditorId = Guid.NewGuid(),
            clipId = clip.clipId,
            userId = userId,
            editedAt = now
        });


        await context.SaveChangesAsync();

        return new UpdateResponseDto
        {
            title = clip.title,
            content = clip.content,
            readOnly = clip.readOnly,
            updatedAt = now
        };
    }
    public async Task<List<GetClipEditorDto>> GetClipEditorsAsync(string slug)
    {

        var editors = await context.ClipEditors
                            .Where(e => e.clipId == slug)
                            .Select(e => new GetClipEditorDto
                            {
                                username = e.User!.username ?? "Anonymous",
                                editedAt = e.editedAt
                            })
                            .ToListAsync();

        return editors;

    }
}