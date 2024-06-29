using AppyNox.Services.Base.Core.AsyncLocals;
using MassTransit;

namespace AppyNox.Services.Base.Infrastructure.MassTransit.Filters;

public class NoxContextFilter : IFilter<SendContext>, IFilter<PublishContext>
{
    public async Task Send(SendContext context, IPipe<SendContext> next)
    {
        ApplyNoxContextToHeaders(context);
        await next.Send(context);
    }

    public async Task Send(PublishContext context, IPipe<PublishContext> next)
    {
        ApplyNoxContextToHeaders(context);
        await next.Send(context);
    }

    private static void ApplyNoxContextToHeaders(SendContext context)
    {
        context.Headers.Set("X-CorrelationId", NoxContext.CorrelationId.ToString());
        context.Headers.Set("X-UserId", NoxContext.UserId.ToString());
        context.Headers.Set("X-CompanyId", NoxContext.CompanyId.ToString());
    }

    private static void ApplyNoxContextToHeaders(PublishContext context)
    {
        context.Headers.Set("X-CorrelationId", NoxContext.CorrelationId.ToString());
        context.Headers.Set("X-UserId", NoxContext.UserId.ToString());
        context.Headers.Set("X-CompanyId", NoxContext.CompanyId.ToString());
    }

    public void Probe(ProbeContext context)
    {
        context.CreateScope("addNoxContextToSendContext");
    }
}
