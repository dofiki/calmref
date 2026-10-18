
using calmref_server.DTO.Clip;
public interface IClipService
{
    Task<GetClipDto> GetClipAsync(string slug, Guid? userId);
    Task<CreateResponseDto> CreateClipAsync(CreateClipDto request, Guid? userId, string slug);
    Task<UpdateResponseDto> UpdateClipAsync(UpdateClipDto request, Guid? userId, string slug);

}