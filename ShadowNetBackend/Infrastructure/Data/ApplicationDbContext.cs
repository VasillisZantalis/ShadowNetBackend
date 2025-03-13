using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ShadowNetBackend.Common;
using ShadowNetBackend.Features.Agents;
using ShadowNetBackend.Features.Communications;
using ShadowNetBackend.Features.Missions;
using ShadowNetBackend.Features.SafeHouses;
using ShadowNetBackend.Features.Witnesses;
using System.Reflection;

namespace ShadowNetBackend.Infrastructure.Data;

public class ApplicationDbContext : IdentityDbContext<Agent>
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

    public DbSet<Agent> Agents { get; set; } = null!;
    public DbSet<Mission> Missions { get; set; } = null!;
    public DbSet<SafeHouse> SafeHouses { get; set; } = null!;
    public DbSet<Witness> Witnesses { get; set; } = null!;
    public DbSet<WitnessRelocation> WitnessRelocations { get; set; } = null!;
    public DbSet<Message> Messages { get; set; } = null!;

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