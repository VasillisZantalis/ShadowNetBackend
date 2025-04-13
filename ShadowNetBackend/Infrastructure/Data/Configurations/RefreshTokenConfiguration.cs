using ShadowNetBackend.Features.Auth.RefreshToken;

namespace ShadowNetBackend.Infrastructure.Data.Configurations;

public class RefreshTokenConfiguration : IEntityTypeConfiguration<RefreshToken>
{
    public void Configure(EntityTypeBuilder<RefreshToken> builder)
    {
        builder.ToTable("RefreshTokens");

        builder.HasKey(rt => rt.Token);

        builder.Property(rt => rt.Expires)
            .IsRequired();

        builder.Property(rt => rt.UserId)
            .IsRequired();
    }
}
