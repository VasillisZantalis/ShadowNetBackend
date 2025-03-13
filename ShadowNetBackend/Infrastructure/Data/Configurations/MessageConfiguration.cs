using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using ShadowNetBackend.Features.Agents;
using ShadowNetBackend.Features.Communications;

namespace ShadowNetBackend.Infrastructure.Data.Configurations;

public class MessageConfiguration : IEntityTypeConfiguration<Message>
{
    public void Configure(EntityTypeBuilder<Message> builder)
    {
        builder.ToTable("Messages");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.SenderId)
            .IsRequired();

        builder.Property(x => x.ReceiverId)
            .IsRequired();

        builder.Property(x => x.Title)
            .IsRequired();

        builder.Property(x => x.Content)
            .IsRequired();

        builder.Property(m => m.SentAt)
            .IsRequired(false);

        builder.Property(m => m.ExpiresAt)
            .IsRequired(false);

        builder.Property(x => x.IsDestroyed)
            .IsRequired(false)
            .HasDefaultValue(false);
    }
}
