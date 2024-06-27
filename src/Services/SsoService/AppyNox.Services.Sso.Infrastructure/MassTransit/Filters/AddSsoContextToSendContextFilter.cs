using AppyNox.Services.Sso.Application.AsyncLocals;
using MassTransit;

namespace AppyNox.Services.Sso.Infrastructure.MassTransit.Filters;

public class AddSsoContextToSendContextFilter : IFilter<SendContext>, IFilter<PublishContext>
{
    public async Task Send(SendContext context, IPipe<SendContext> next)
    {
        ApplySsoContextToHeaders(context);
        await next.Send(context);
    }

    public async Task Send(PublishContext context, IPipe<PublishContext> next)
    {
        ApplySsoContextToHeaders(context);
        await next.Send(context);
    }

    private void ApplySsoContextToHeaders(SendContext context)
    {
        context.Headers.Set("X-CompanyId", SsoContext.CompanyId.ToString());
        context.Headers.Set("X-IsAdmin", SsoContext.IsAdmin.ToString());
        context.Headers.Set("X-IsSuperAdmin", SsoContext.IsSuperAdmin.ToString());
        context.Headers.Set("X-IsConnectRequest", SsoContext.IsConnectRequest.ToString());
    }

    private void ApplySsoContextToHeaders(PublishContext context)
    {
        context.Headers.Set("X-CompanyId", SsoContext.CompanyId.ToString());
        context.Headers.Set("X-IsAdmin", SsoContext.IsAdmin.ToString());
        context.Headers.Set("X-IsSuperAdmin", SsoContext.IsSuperAdmin.ToString());
        context.Headers.Set("X-IsConnectRequest", SsoContext.IsConnectRequest.ToString());
    }

    public void Probe(ProbeContext context)
    {
        context.CreateScope("addSsoContextToSendContext");
    }
}
