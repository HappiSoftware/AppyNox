using AppyNox.Services.Sso.Infrastructure.AsyncLocals;
using MassTransit;

namespace AppyNox.Services.Sso.Infrastructure.MassTransit.Filters;

public class SsoContextConsumeFilter<T> : IFilter<ConsumeContext<T>> where T : class
{
    public async Task Send(ConsumeContext<T> context, IPipe<ConsumeContext<T>> next)
    {

        _ = Guid.TryParse(context.Headers.Get("X-CompanyId", Guid.Empty.ToString()), out Guid companyId);
        _ = bool.TryParse(context.Headers.Get("X-IsAdmin", "false"), out bool isAdmin);
        _ = bool.TryParse(context.Headers.Get("X-IsSuperAdmin", "false"), out bool isSuperAdmin);
        _ = bool.TryParse(context.Headers.Get("X-IsConnectRequest", "false"), out bool isConnectRequest);

        SsoContext.CompanyId = companyId;
        SsoContext.IsAdmin = isAdmin;
        SsoContext.IsSuperAdmin = isSuperAdmin;
        SsoContext.IsConnectRequest = isConnectRequest;

        try
        {
            await next.Send(context);
        }
        finally
        {
            // Clear the SsoContext after processing
            SsoContext.CompanyId = Guid.Empty;
            SsoContext.IsAdmin = false;
            SsoContext.IsSuperAdmin = false;
            SsoContext.IsConnectRequest = false;
        }
    }

    public void Probe(ProbeContext context)
    {
        context.CreateScope("ssoContextConsumeFilter");
    }
}
