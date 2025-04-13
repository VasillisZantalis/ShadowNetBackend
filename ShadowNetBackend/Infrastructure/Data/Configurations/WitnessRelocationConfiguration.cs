namespace ShadowNetBackend.Infrastructure.Data.Configurations;

public class WitnessRelocationConfiguration : IEntityTypeConfiguration<WitnessRelocation>
{
    public void Configure(EntityTypeBuilder<WitnessRelocation> builder)
    {
        builder.ToTable("WitnessRelocations");

        builder.HasKey(wr => wr.Id);

        builder.HasOne(wr => wr.Witness)
            .WithMany(w => w.Relocations)
            .HasForeignKey(wr => wr.WitnessId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(wr => wr.SafeHouse)
            .WithMany(sh => sh.WitnessRelocations)
            .HasForeignKey(wr => wr.SafeHouseId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Property(wr => wr.RelocationDate)
            .IsRequired();
    }
}
