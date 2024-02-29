using AppyNox.Services.Coupon.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AppyNox.Services.Coupon.Infrastructure.Data.Configurations;

internal class TicketTagConfiguration(Guid ticketTagId, Guid ticketId) : IEntityTypeConfiguration<TicketTag>
{
    public void Configure(EntityTypeBuilder<TicketTag> builder)
    {
        #region [ Configuration ]

        builder.HasKey(t => t.Id);
        builder.Property(t => t.Description).IsRequired();

        #endregion

        #region [ SeedData ]

        builder.HasData(new TicketTag()
        {
            Id = ticketTagId,
            Description = "Tag Description",
            TicketId = ticketId
        });

        #endregion
    }
}