using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ShadowNetBackend.Infrastructure.Data.Configurations;

public class AgentConfiguration : IEntityTypeConfiguration<Agent>
{
    public void Configure(EntityTypeBuilder<Agent> builder)
    {
        builder.ToTable("Agents");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.FirstName)
            .IsRequired()
            .HasMaxLength(250);

        builder.Property(x => x.LastName)
            .IsRequired()
            .HasMaxLength(250);

        builder.Property(m => m.Image)
            .IsRequired(false);

        builder.Property(x => x.Specialization)
            .IsRequired(false);

        builder.Property(x => x.Alias)
            .IsRequired(false);

        builder.Property(x => x.ClearanceLevel)
            .IsRequired();

        builder.HasOne(a => a.Mission)
            .WithMany(ma => ma.AssignedAgents)
            .HasForeignKey(a => a.MissionId)
            .OnDelete(DeleteBehavior.SetNull);
    }
}
