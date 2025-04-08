using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ShadowNetBackend.Infrastructure.Data.Configurations;

public class SafeHouseConfiguration : IEntityTypeConfiguration<SafeHouse>
{
    public void Configure(EntityTypeBuilder<SafeHouse> builder)
    {
        builder.ToTable("SafeHouses");

        builder.HasKey(sh => sh.Id);

        builder.Property(sh => sh.Location)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(sh => sh.Capacity)
            .IsRequired();

        builder.Property(sh => sh.IsActive)
            .IsRequired();

        builder.HasMany(sh => sh.WitnessRelocations)
            .WithOne(wr => wr.SafeHouse)
            .HasForeignKey(wr => wr.SafeHouseId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
