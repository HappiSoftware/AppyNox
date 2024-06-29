namespace AppyNox.Services.License.Contarcts.MassTransit
{
    public interface ILoadServiceClient
    {
        Task<Guid> GetLicenseById(string key);
    }
}