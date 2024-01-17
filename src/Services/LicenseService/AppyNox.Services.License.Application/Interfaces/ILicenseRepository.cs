using AppyNox.Services.Base.Application.Interfaces.Repositories;
using AppyNox.Services.License.Domain.Entities;

namespace AppyNox.Services.License.Application.Interfaces
{
    public interface ILicenseRepository
    {
        #region [ Public Methods ]

        Task AssignLicenseToApplicationUserAsync(Guid licenseId, Guid applicationUserId);

        Task<LicenseEntity?> FindLicenseByKeyAsync(string licenseKey, CancellationToken cancellationToken);

        Task<int> GetUserCountForLicenseKeyAsync(Guid licenseId, CancellationToken cancellationToken);

        #endregion
    }
}