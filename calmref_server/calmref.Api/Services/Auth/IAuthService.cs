using calmref_server.DTO.Auth;

public interface IAuthService
{
    public Task<RegisterResponseDto> RegisterAsync(UserDto request);

    public Task<RegisterResponseDto> VerifyOtpAsync(VerifyOtpDto request);

    public Task<TokenResponseDto> LoginAsync(UserDto request);

    public Task<TokenResponseDto> RefreshTokenAsync(RefreshTokenRequestDto request);
}