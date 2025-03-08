using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using ShadowNetBackend.Features.Missions;

namespace ShadowNetBackend.Infrastructure.Data.Configurations;

public class MissionConfiguration : IEntityTypeConfiguration<Mission>
{
    public void Configure(EntityTypeBuilder<Mission> builder)
    {
        builder.ToTable("Missions");

        builder.HasKey(m => m.Id);

        builder.Property(m => m.Title)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(m => m.Image)
            .IsRequired(false);

        builder.Property(m => m.Objective)
            .IsRequired();

        builder.Property(m => m.Location)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(m => m.Status)
            .IsRequired();

        builder.Property(m => m.Risk)
            .IsRequired();

        builder.Property(m => m.Date)
            .IsRequired();

        builder.HasMany(m => m.AssignedAgents)
            .WithOne(ma => ma.Mission)
            .HasForeignKey(ma => ma.MissionId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
