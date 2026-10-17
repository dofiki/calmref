using DTO;
using calmref_server.Models;
using calmref_server.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity.Data;
public interface IAuthService
{
    public Task<RegisterResponseDto> RegisterAsync(UserDto request);

    public Task<RegisterResponseDto> VerifyOtpAsync(VerifyOtpDto request);

    public Task<TokenResponseDto> LoginAsync(UserDto request);

    public Task<TokenResponseDto> RefreshTokenAsync(RefreshTokenRequestDto request);
}