using AppyNox.Services.Base.Core.AsyncLocals;
using MassTransit;

namespace AppyNox.Services.Base.Infrastructure.MassTransit.Filters;

public class NoxContextConsumeFilter<T> : IFilter<ConsumeContext<T>> where T : class
{
    public async Task Send(ConsumeContext<T> context, IPipe<ConsumeContext<T>> next)
    {

        _ = Guid.TryParse(context.Headers.Get("X-CorrelationId", Guid.Empty.ToString()), out Guid correlationId);
        _ = Guid.TryParse(context.Headers.Get("X-UserId", Guid.Empty.ToString()), out Guid userId);
        _ = Guid.TryParse(context.Headers.Get("X-CompanyId", Guid.Empty.ToString()), out Guid companyId);

        NoxContext.CorrelationId = correlationId;
        NoxContext.UserId = userId;
        NoxContext.CompanyId = companyId;

        try
        {
            await next.Send(context);
        }
        finally
        {
            // Clear the NoxContext after processing
            NoxContext.CorrelationId = Guid.Empty;
            NoxContext.UserId = Guid.Empty;
            NoxContext.CompanyId = Guid.Empty;
        }
    }

    public void Probe(ProbeContext context)
    {
        context.CreateScope("noxContextConsumeFilter");
    }
}