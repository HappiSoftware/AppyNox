using AppyNox.Services.Sso.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AppyNox.Services.Sso.Infrastructure.Data.Configurations;

internal sealed class EmailProviderConfiguration : IEntityTypeConfiguration<EmailProvider>
{
    public void Configure(EntityTypeBuilder<EmailProvider> builder)
    {
        builder.HasKey(e => e.Id);

        builder.Property(x => x.Host).IsRequired();
        builder.Property(x => x.Port).IsRequired();
        builder.Property(x => x.Username).IsRequired();
        builder.Property(x => x.Password).IsRequired();
        builder.Property(x => x.UseSSL).IsRequired();
        builder.Property(x => x.FromAddress).IsRequired();
        builder.Property(x => x.FromName).IsRequired();
    }
}