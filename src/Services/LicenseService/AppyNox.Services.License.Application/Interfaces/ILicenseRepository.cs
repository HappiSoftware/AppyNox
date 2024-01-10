using AppyNox.Services.Base.Application.Interfaces.Repositories;
using AppyNox.Services.License.Domain.Entities;

namespace AppyNox.Services.License.Application.Interfaces
{
    public interface ILicenseRepository
    {
        Task AssignLicenseToApplicationUser(Guid applicationUserId, Guid licenseId);
        #region Public Methods

        Task<LicenseEntity?> FindLicenseByKey(string licenseKey, CancellationToken cancellationToken);

        Task<int> GetUserCountForLicenseKey(Guid licenseId, CancellationToken cancellationToken);

        #endregion
    }
}