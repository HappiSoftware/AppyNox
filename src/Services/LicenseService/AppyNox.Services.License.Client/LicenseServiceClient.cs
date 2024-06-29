using AppyNox.Services.License.Contarcts.MassTransit.Contracts;
using MassTransit;

namespace AppyNox.Services.License.Client;

public class LicenseServiceClient(IBus bus) : ILicenseServiceClient
{
    private readonly IBus _bus = bus;

    public async Task<Guid> GetLicenseById(string key)
    {
        var client = _bus.CreateRequestClient<GetLicenseIdByKeyDataRequest>();

        var response = await client.GetResponse<GetLicenseIdByKeyDataResponse>(
            new GetLicenseIdByKeyDataRequest(key)
            );
        return response.Message.LicenseId;
    }
}