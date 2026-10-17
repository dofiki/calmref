using DTO;
using calmref_server.Models;
using calmref_server.Data;
using System.Security.Cryptography;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.IdentityModel.Tokens.Jwt;

public class AuthService(CrefDbContext context, IEmailSender emailSender, IConfiguration config) : IAuthService
{
    private const int OtpExpiryMinutes = 10;
    private const int MaxOtpAttempts = 5;
    private const int LockMinutes = 10;
    private const int AccessTokenExpirationDay = 1;
    private const int RefreshTokenExpirationDay = 7;

    private static string BuildOtpEmail(string username, string otp)
    {
        return $"""
        Dear {username},

        Your OTP code is {otp}.

        This code will expire in {OtpExpiryMinutes} minutes.

        Regards,
        Support Team
        """;
    }

    private static string GenerateAndSaveOtp(User user, PasswordHasher<User> hasher)
    {
        var otp = RandomNumberGenerator.GetInt32(0, 1_000_000).ToString("D6");

        if (user.EmailOtp is null)
        {
            user.EmailOtp = new EmailOtp { userId = user.userId };
        }

        user.EmailOtp.hashedOtp = hasher.HashPassword(user, otp);
        user.EmailOtp.OtpExpiresAt = DateTime.UtcNow.AddMinutes(OtpExpiryMinutes);
        user.EmailOtp.failedAttempts = 0;
        user.EmailOtp.LockedUntil = null;
        return otp;
    }

    private static void EnsureUserCanReRegister(User user)
    {
        var now = DateTime.UtcNow;

        if (user.emailVerified) throw new
                ConflictException("Account Already Registered and Verified.");

        if (user.EmailOtp?.LockedUntil > now)
            throw new TooManyAttemptsException("Too many attempts. Please try again later.");

        if (user.EmailOtp?.OtpExpiresAt > now)
            throw new ConflictException("A code was already sent. Check your email, or try again after it expires.");
    }

    private static void EnsureUserCanVerifyOtp(User user)
    {
        var now = DateTime.UtcNow;

        if (user is null) throw new NotFoundException("No account Found !");

        if (user?.EmailOtp is null)
            throw new ApplicationException("Invalid or expired code.");

        if (user.EmailOtp.LockedUntil > now)
        {
            throw new ApplicationException("Too many attempts. Try again later.");
        }

        if (user.EmailOtp.OtpExpiresAt < now)
        {
            throw new ApplicationException("Invalid or expired code.");
        }
    }

    private static User CreateUser(string username, string email,
            string password, PasswordHasher<User> hasher, CrefDbContext context)
    {
        var user = new User
        {
            userId = Guid.NewGuid(),
            username = username,
            email = email,
            emailVerified = false,
            createdAt = DateTime.UtcNow
        };

        var UserCredential = new UserCredential
        {
            userId = user.userId,
            hashedPassword = hasher.HashPassword(user, password)
        };

        user.UserCredential = UserCredential;
        context.Users.Add(user);

        return user;
    }

    public async Task<RegisterResponseDto> RegisterAsync(UserDto request)
    {

        var email = request.email.Trim().ToLowerInvariant();
        var username = email.Split("@")[0];
        var hasher = new PasswordHasher<User>();

        var user = await context.Users
            .Include(u => u.UserCredential)
            .Include(u => u.EmailOtp)
            .FirstOrDefaultAsync(u => u.email == email);

        if (user is null)
        {
            user = CreateUser(username, email, request.password, hasher, context);
        }
        else
        {
            EnsureUserCanReRegister(user);
            // incase user changes their password again
            user.UserCredential!.hashedPassword = hasher.HashPassword(user, request.password);
        }

        var otp = GenerateAndSaveOtp(user, hasher);
        await emailSender.SendAsync(email, "Verification Code", BuildOtpEmail(username, otp));
        await context.SaveChangesAsync();

        return new RegisterResponseDto
        {
            userId = user.userId,
            message = "Please check your email for OTP code."
        };

    }

    public async Task<RegisterResponseDto> VerifyOtpAsync(VerifyOtpDto request)
    {
        var user = await context.Users
                .Include(u => u.EmailOtp)
                .FirstOrDefaultAsync(u => u.userId == request.userId);

        EnsureUserCanVerifyOtp(user);

        // check otp 
        var EmailStatus = new PasswordHasher<User>()
            .VerifyHashedPassword(user, user.EmailOtp.hashedOtp, request.otp.Trim());

        // invalid or expired otp
        if (EmailStatus != PasswordVerificationResult.Success)
        {
            user.EmailOtp.failedAttempts += 1;

            if (user.EmailOtp.failedAttempts >= MaxOtpAttempts)
                user.EmailOtp.LockedUntil = DateTime.UtcNow.AddMinutes(LockMinutes);

            await context.SaveChangesAsync();

            // after saving the failed attempts and LockedUntil to db we throw error
            if (user.EmailOtp.LockedUntil is not null)
                throw new TooManyAttemptsException($"Too many attempts. Locked for {LockMinutes} minutes.");
            else throw new BadRequestException("Invalid or expired OTP");

        }

        // valid otp
        user.emailVerified = true;
        context.Remove(user.EmailOtp);
        await context.SaveChangesAsync();

        return new RegisterResponseDto
        {
            userId = user.userId,
            message = "Your email has been Verified."
        };
    }

    public async Task<TokenResponseDto> LoginAsync(UserDto request)
    {

        var usr = await context.Users
                        .Include(usr => usr.UserCredential)
                        .FirstOrDefaultAsync(usr => usr.email == request.email);

        if (usr is null)
        {
            throw new NotFoundException("User not found");
        }

        if (usr.emailVerified == false)
        {
            throw new BadRequestException("Verify your email first");
        }

        var authStatus = new PasswordHasher<User>().
        VerifyHashedPassword(usr, usr.UserCredential!.hashedPassword, request.password);

        if (authStatus == PasswordVerificationResult.Failed)
        {
            throw new BadRequestException("Wrong credential/s");
        }

        return await GenerateTokens(usr);
    }

    public async Task<TokenResponseDto> GenerateTokens(User usr)
    {
        return new TokenResponseDto
        {
            accessToken = GenerateAccessToken(usr),
            refreshToken = await GenerateRefreshToken(usr)
        };

    }

    private string GenerateAccessToken(User usr)
    {
        List<Claim> claims = new List<Claim>
         {
           new Claim(ClaimTypes.NameIdentifier, usr.userId.ToString()),
           new Claim(ClaimTypes.Name, usr.username)
         };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config.GetValue<string>("Application:Token")!));

        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var tokenDescriptor = new JwtSecurityToken(
            issuer: config.GetValue<string>("Application:Issuer"),
            audience: config.GetValue<string>("Application:Audience"),
            claims: claims,
            expires: DateTime.UtcNow.AddDays(AccessTokenExpirationDay),
            signingCredentials: creds
        );

        return new JwtSecurityTokenHandler().WriteToken(tokenDescriptor);
    }

    private async Task<string> GenerateRefreshToken(User usr)
    {

        var randomBytes = RandomNumberGenerator.GetBytes(32);
        var refreshToken = Convert.ToBase64String(randomBytes);

        var hashedRefreshToken = new PasswordHasher<User>().HashPassword(usr, refreshToken);

        usr.UserCredential!.hashedRefreshToken = hashedRefreshToken;
        usr.UserCredential.RefreshTokenExpiresAt = DateTime.UtcNow.AddDays(RefreshTokenExpirationDay);

        await context.SaveChangesAsync();

        return refreshToken;
    }

    public async Task<TokenResponseDto> RefreshTokenAsync(RefreshTokenRequestDto request)
    {
        var usr = await context.Users
                    .Include(usr => usr.UserCredential)
                    .FirstOrDefaultAsync(usr => usr.userId == request.userId);

        if (usr is null || usr.UserCredential is null)
        {
            throw new NotFoundException("User not found!");
        }

        var tokenStatus = new PasswordHasher<User>()
            .VerifyHashedPassword(usr, usr.UserCredential.hashedRefreshToken, request.RefreshToken);

        var now = DateTime.UtcNow;

        if (tokenStatus == PasswordVerificationResult.Failed || usr.UserCredential.RefreshTokenExpiresAt < now)
        {
            throw new BadRequestException("Expired refresh token");
        }

        return new TokenResponseDto
        {
            accessToken = GenerateAccessToken(usr),
            refreshToken = await GenerateRefreshToken(usr)

        };


    }
}