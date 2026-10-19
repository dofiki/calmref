using calmref_server.DTO.User;
public interface IUserService
{
    Task<GetUserDto> GetUserAsync(Guid userId);
    Task<List<GetUserClipsDto>> GetUserClipsAsync(Guid userId);
}