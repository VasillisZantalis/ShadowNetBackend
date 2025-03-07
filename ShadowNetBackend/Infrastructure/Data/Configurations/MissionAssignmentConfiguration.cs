using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using ShadowNetBackend.Features.Missions;

namespace ShadowNetBackend.Infrastructure.Data.Configurations;

public class MissionAssignmentConfiguration : IEntityTypeConfiguration<MissionAssignment>
{
    public void Configure(EntityTypeBuilder<MissionAssignment> builder)
    {
        builder.ToTable("MissionAssignments");

        builder.HasKey(ma => ma.Id);

        builder.HasIndex(ma => new { ma.AgentId, ma.MissionId })
            .IsUnique();

        builder.HasOne(ma => ma.Agent)
            .WithMany(a => a.Assignments)
            .HasForeignKey(ma => ma.AgentId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(ma => ma.Mission)
            .WithMany(m => m.AssignedAgents)
            .HasForeignKey(ma => ma.MissionId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Property(ma => ma.AssignedDate)
            .IsRequired();
    }
}
