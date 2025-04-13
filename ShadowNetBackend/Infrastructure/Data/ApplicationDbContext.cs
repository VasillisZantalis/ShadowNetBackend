using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using ShadowNetBackend.Features.Auth.RefreshToken;
using ShadowNetBackend.Features.Criminals;
using ShadowNetBackend.Features.Messages;
using System.Reflection;

namespace ShadowNetBackend.Infrastructure.Data;

public class ApplicationDbContext : IdentityDbContext<Agent>
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

    public DbSet<Agent> Agents => Set<Agent>();
    public DbSet<Mission> Missions => Set<Mission>();
    public DbSet<SafeHouse> SafeHouses => Set<SafeHouse>();
    public DbSet<Witness> Witnesses => Set<Witness>();
    public DbSet<WitnessRelocation> WitnessRelocations => Set<WitnessRelocation>();
    public DbSet<Message> Messages => Set<Message>();
    public DbSet<Criminal> Criminals => Set<Criminal>();
    public DbSet<RefreshToken> RefreshTokens => Set<RefreshToken>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

        // Seed Roles
        var identityRoles = Enum.GetValues(typeof(UserRank))
            .Cast<UserRank>()
            .Select(role => new IdentityRole
            {
                Id = ((short)role).ToString(),
                Name = role.ToString(),
                NormalizedName = role.ToString().ToUpper()
            })
            .ToArray();

        modelBuilder.Entity<IdentityRole>().HasData(identityRoles);
    }
}