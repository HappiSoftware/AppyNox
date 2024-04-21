using AppyNox.Services.Coupon.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AppyNox.Services.Coupon.Infrastructure.Data.Configurations;

internal class TicketConfiguration(Guid ticketId) : IEntityTypeConfiguration<Ticket>
{
    public void Configure(EntityTypeBuilder<Ticket> builder)
    {
        #region [ Configuration ]

        builder.HasKey(t => t.Id);
        builder.Property(t => t.Title).IsRequired();
        builder.Property(t => t.Content).IsRequired();
        builder.Property(t => t.ReportDate).IsRequired();

        builder.HasMany(t => t.Tags)
            .WithOne(tg => tg.Ticket)
            .HasForeignKey(tt => tt.TicketId)
            .OnDelete(DeleteBehavior.Cascade);

        #endregion

        #region [ SeedData ]

        builder.HasData(new
        {
            Id = ticketId,
            Title = "Title",
            Content = "Ticket content",
            ReportDate = DateTime.SpecifyKind(new DateTime(2024, 4, 21), DateTimeKind.Utc),
            CreatedBy = "System",
            CreationDate = DateTime.SpecifyKind(new DateTime(2024, 4, 21), DateTimeKind.Utc),
            UpdatedBy = (string?)null,
            UpdateDate = (DateTime?)null
        });

        #endregion
    }
}