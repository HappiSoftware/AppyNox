using AppyNox.Services.Coupon.Domain.Entities;
using AppyNox.Services.Coupon.Infrastructure.Data;
using AppyNox.Services.Coupon.Infrastructure.Repositories;

namespace AppyNox.Services.Coupon.Infrastructure.UnitTest.Seeds;

internal static class TicketTestSeedData
{
    #region [ Properties ]

    private static int TitleSuffix { get; set; } = 1;

    private static int ContentSuffix { get; set; } = 1;

    private static int TagSuffix { get; set; } = 1;

    #endregion

    #region [ Internal Methods ]

    internal static async Task<Ticket> SeedOneTicket(this CouponDbContext context, UnitOfWork unitOfWork)
    {
        return (await context.SeedMultipleTickets(unitOfWork, 1, 1)).First();
    }

    internal static async Task<IEnumerable<Ticket>> SeedMultipleTickets(this CouponDbContext context, UnitOfWork unitOfWork, int ticketSize, int ticketTagSize)
    {
        if (ticketSize <= 0)
        {
            throw new ArgumentException("Ticket size must be greater than 0.", nameof(ticketSize));
        }
        if (ticketTagSize <= 0)
        {
            throw new ArgumentException("TicketTag size must be greater than 0.", nameof(ticketTagSize));
        }

        var tickets = new List<Ticket>();
        Random random = new();

        #region [ TicketTags ]

        #endregion

        #region [ Tickets ]

        int titleSuffix = TitleSuffix;
        int contentSuffix = ContentSuffix;

        for (int i = 0; i < ticketSize; i++)
        {
            // Create ticket now
            Ticket ticket = new()
            {
                Title = $"Title {titleSuffix++}",
                Content = $"Content {contentSuffix++}",
                ReportDate = DateTime.Now,
                Tags = null
            };

            context.Tickets.Add(ticket);
            await unitOfWork.SaveChangesAsync();

            // Random amount of ticket tags
            int tagSuffix = TagSuffix;
            var ticketTags = new List<TicketTag>();
            int ticketTagSizeIndex = random.Next(1, ticketTagSize + 1);
            for (int j = 0; j < ticketTagSizeIndex; j++)
            {
                TicketTag ticketTag = new()
                {
                    Description = $"Tag Description{tagSuffix++}",
                    TicketId = ticket.Id,
                    Ticket = ticket
                };
                ticketTags.Add(ticketTag);
            }

            await context.TicketTags.AddRangeAsync(ticketTags);
            await unitOfWork.SaveChangesAsync();

            tickets.Add(ticket);
        }

        #endregion

        return tickets;
    }

    #endregion
}