using calmref_server.Models;
using Microsoft.EntityFrameworkCore;

namespace calmref_server.Data;

public class CrefDbContext : DbContext
{
    public CrefDbContext(DbContextOptions<CrefDbContext> options) : base(options) { }
    public DbSet<User> Users => Set<User>();
    public DbSet<EmailOtp> EmailOtps => Set<EmailOtp>();
    public DbSet<UserCredential> UserCredentials => Set<UserCredential>();
    public DbSet<Clip> Clips => Set<Clip>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {

        modelBuilder.Entity<User>()
               .HasIndex(u => u.email)
               .IsUnique();

        modelBuilder.Entity<User>()
                    .HasOne(e => e.EmailOtp)
                    .WithOne(e => e.User)
                    .HasForeignKey<EmailOtp>(e => e.userId);

        modelBuilder.Entity<User>()
                    .HasOne(e => e.UserCredential)
                    .WithOne(e => e.User)
                    .HasForeignKey<UserCredential>(e => e.userId);

        modelBuilder.Entity<User>()
                    .HasMany(e => e.Clips)
                    .WithOne(e => e.User)
                    .HasForeignKey(e => e.userId);
    }
}