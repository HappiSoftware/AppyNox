using AppyNox.Services.License.Contarcts.MassTransit.Contracts;
using MassTransit;

namespace AppyNox.Services.License.Infrastructure.MassTransit.Consumers;

public class GetLicenseIdByKeyDataRequestConsumer : IConsumer<GetLicenseIdByKeyDataRequest>
{

    public Task Consume(ConsumeContext<GetLicenseIdByKeyDataRequest> context)
    {
        GetLicenseIdByKeyDataRequest message = context.Message;
        throw new NotImplementedException();
    }
}