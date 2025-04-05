using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ShadowNetBackend.Features.Criminals;

namespace ShadowNetBackend.Infrastructure.Data.Configurations;

public class CriminalConfiguration : IEntityTypeConfiguration<Criminal>
{
    public void Configure(EntityTypeBuilder<Criminal> builder)
    {
        builder.ToTable("Criminals");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.FirstName)
            .IsRequired()
            .HasMaxLength(250);

        builder.Property(x => x.LastName)
            .IsRequired()
            .HasMaxLength(250);

        builder.Property(x => x.Alias)
            .IsRequired(false)
            .HasMaxLength(250);

        builder.Property(x => x.ThreatLevel)
            .IsRequired()
            .HasDefaultValue(RiskLevel.Low);

        builder.Property(m => m.Image)
            .IsRequired(false);

        builder.Property(m => m.DateOfBirth)
            .IsRequired(false);

        builder.Property(m => m.LastSpottedDate)
            .IsRequired(false);

        builder.Property(x => x.Nationality)
            .IsRequired(false)
            .HasMaxLength(250);

        builder.Property(x => x.KnownAffiliations)
            .IsRequired(false)
            .HasMaxLength(250);

        builder.Property(x => x.LastKnownLocation)
            .IsRequired(false)
            .HasMaxLength(250);

        builder.Property(x => x.SurveillanceNotes)
            .IsRequired(false);

        builder.Property(x => x.IsArmedAndDangerous)
            .IsRequired()
            .HasDefaultValue(false);

        builder.Property(x => x.UnderSurveillance)
            .IsRequired()
            .HasDefaultValue(false);
    }
}
