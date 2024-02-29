using AppyNox.Services.Coupon.Domain.Entities;
using AppyNox.Services.Coupon.Infrastructure.Data;

namespace AppyNox.Services.Coupon.Infrastructure.UnitTest.Seeds;

internal static class TicketTestSeedData
{
    #region [ Properties ]

    private static int TitleSuffix { get; set; } = 1;

    private static int ContentSuffix { get; set; } = 1;

    private static int TagSuffix { get; set; } = 1;

    #endregion

    #region [ Internal Methods ]

    internal static Ticket SeedOneTicket(this CouponDbContext context)
    {
        return context.SeedMultipleTickets(1, 1).First();
    }

    internal static IEnumerable<Ticket> SeedMultipleTickets(this CouponDbContext context, int ticketSize, int ticketTagSize)
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
            // Random amount of ticket tags
            int tagSuffix = TagSuffix;
            var ticketTags = new List<TicketTag>();
            int ticketTagSizeIndex = random.Next(1, ticketTagSize + 1);
            for (int j = 0; j < ticketTagSizeIndex; j++)
            {
                TicketTag ticketTag = new()
                {
                    Description = $"Tag Description{tagSuffix++}"
                };
                ticketTags.Add(ticketTag);
            }

            // Create ticket now
            Ticket ticket = new()
            {
                Title = $"Title {titleSuffix++}",
                Content = $"Content {contentSuffix++}",
                ReportDate = DateTime.Now,
                Tags = ticketTags
            };

            tickets.Add(ticket);
        }
        context.Tickets.AddRange(tickets);
        context.SaveChanges();

        #endregion

        return tickets;
    }

    #endregion
}