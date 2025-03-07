using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using ShadowNetBackend.Features.Witnesses;

namespace ShadowNetBackend.Infrastructure.Data.Configurations;

public class WitnessConfiguration : IEntityTypeConfiguration<Witness>
{
    public void Configure(EntityTypeBuilder<Witness> builder)
    {
        builder.ToTable("Witnesses");

        builder.HasKey(w => w.Id);

        builder.Property(w => w.Alias)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(w => w.LocationHistory)
            .HasMaxLength(500);

        builder.Property(w => w.RiskLevel)
            .IsRequired();

        builder.Property(w => w.RelocationStatus)
            .IsRequired();

        builder.HasMany(w => w.Relocations)
            .WithOne(wr => wr.Witness)
            .HasForeignKey(wr => wr.WitnessId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
