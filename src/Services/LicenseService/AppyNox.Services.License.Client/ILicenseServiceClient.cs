
namespace AppyNox.Services.License.Client
{
    public interface ILicenseServiceClient
    {
        Task<Guid> GetLicenseById(string key);
    }
}