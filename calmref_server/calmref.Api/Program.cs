using calmref_server.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.Security.Cryptography;
using System.Threading.RateLimiting;

var builder = WebApplication.CreateBuilder(args);

var ImgKitPrivateKey = builder.Configuration["IMAGEKIT_PRIVATE_KEY"]
    ?? throw new InvalidOperationException("IMAGEKIT_PRIVATE_KEY is not set");

builder.Services.AddControllers();

builder.Services.AddDbContext<CrefDbContext>(options =>
{
    options.UseMySQL(builder.Configuration.GetConnectionString("Default")!);
});

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Application:Issuer"],
            ValidAudience = builder.Configuration["Application:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(builder.Configuration["Application:Token"]!))

        };
    });

builder.Services.AddProblemDetails();
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();

builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IClipService, ClipService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddTransient<IEmailSender, EmailSender>();
builder.Services.AddTransient<ISmtpService, SmtpService>();

builder.Services.AddCors(options =>
    options.AddPolicy("frontend", policy =>
        policy.WithOrigins("http://localhost:5173")
              .AllowAnyHeader()
              .AllowAnyMethod()));

builder.Services.AddRateLimiter(options =>
{
    options.RejectionStatusCode = StatusCodes.Status429TooManyRequests;
    options.AddPolicy("imgauth", httpContext =>
     RateLimitPartition.GetFixedWindowLimiter(
         httpContext.Connection.RemoteIpAddress?.ToString() ?? "unknown",
         _ => new FixedWindowRateLimiterOptions
         {
             PermitLimit = 5,
             Window = TimeSpan.FromMinutes(1),
             QueueLimit = 0
         }
     ));
});

var app = builder.Build();

app.UseHttpsRedirection();
app.UseExceptionHandler();
app.UseCors("frontend");
app.UseRateLimiter();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.MapGet("/imgauth", () =>
{
    var token = Guid.NewGuid().ToString();
    var expire = DateTimeOffset.UtcNow.ToUnixTimeSeconds() + 600;

    using var hmac = new HMACSHA1(Encoding.UTF8.GetBytes(ImgKitPrivateKey));
    var hash = hmac.ComputeHash(Encoding.UTF8.GetBytes(token + expire));
    var signature = Convert.ToHexString(hash).ToLowerInvariant();

    return Results.Ok(new { token, expire, signature });
}).RequireRateLimiting("imgauth");

app.Run();