using AppyNox.Services.Base.Application.Interfaces.Loggers;
using AppyNox.Services.Base.Domain.Interfaces;
using AppyNox.Services.Base.Infrastructure.Repositories;
using AppyNox.Services.License.Application.Interfaces;
using AppyNox.Services.License.Domain.Entities;
using AppyNox.Services.License.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace AppyNox.Services.License.Infrastructure.Repositories
{
    internal class LicenseRepository<TEntity>(LicenseDatabaseContext context, INoxInfrastructureLogger noxInfrastructureLogger)
        : GenericRepositoryBase<TEntity>(context, noxInfrastructureLogger), ILicenseRepository where TEntity : class, IEntityWithGuid
    {
        #region [ Fields ]

        private readonly LicenseDatabaseContext _context = context;

        #endregion

        #region Public Methods

        public async Task<LicenseEntity?> FindLicenseByKey(string licenseKey, CancellationToken cancellationToken)
        {
            return await _context.Licenses
                                        .FirstOrDefaultAsync(l => l.LicenseKey == licenseKey, cancellationToken);
        }

        public async Task<int> GetUserCountForLicenseKey(Guid licenseId, CancellationToken cancellationToken)
        {
            return await _context.ApplicationUserLicenses
                                          .CountAsync(ul => ul.LicenseId == licenseId, cancellationToken);
        }

        #endregion
    }
}