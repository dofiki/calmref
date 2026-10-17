using DTO;
using Microsoft.AspNetCore.Mvc;

[Route("api/[controller]")]
[ApiController]
public class AuthController(IAuthService authService) : ControllerBase
{
    [HttpPost("register")]
    public async Task<ActionResult<RegisterResponseDto>> Register(UserDto request)
        => Ok(await authService.RegisterAsync(request));

    [HttpPost("verify-otp")]
    public async Task<ActionResult<RegisterResponseDto>> VerifyOtp(VerifyOtpDto request)
        => Ok(await authService.VerifyOtpAsync(request));

    [HttpPost("login")]
    public async Task<ActionResult<TokenResponseDto>> Login(UserDto request)
        => Ok(await authService.LoginAsync(request));

    [HttpPost("refresh-token")]
    public async Task<ActionResult<TokenResponseDto>> RefreshToken(RefreshTokenRequestDto request)
        => Ok(await authService.RefreshTokenAsync(request));

}