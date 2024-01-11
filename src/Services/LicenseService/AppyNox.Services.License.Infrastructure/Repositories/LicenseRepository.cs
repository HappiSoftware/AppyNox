using AppyNox.Services.Base.Application.Interfaces.Loggers;
using AppyNox.Services.Base.Application.Interfaces.Repositories;
using AppyNox.Services.Base.Infrastructure.Repositories;
using AppyNox.Services.License.Application.Interfaces;
using AppyNox.Services.License.Domain.Entities;
using AppyNox.Services.License.Infrastructure.Data;
using AppyNox.Services.License.Infrastructure.ExceptionExtensions;
using Microsoft.EntityFrameworkCore;

namespace AppyNox.Services.License.Infrastructure.Repositories
{
    internal class LicenseRepository(LicenseDatabaseContext context, INoxInfrastructureLogger noxInfrastructureLogger, IUnitOfWorkBase unitOfWork)
        : GenericRepositoryBase<LicenseEntity>(context, noxInfrastructureLogger), ILicenseRepository
    {
        #region [ Fields ]

        private readonly LicenseDatabaseContext _context = context;

        private readonly IUnitOfWorkBase _unitOfWork = unitOfWork;

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

        public async Task AssignLicenseToApplicationUser(Guid licenseId, Guid applicationUserId)
        {
            try
            {
                ApplicationUserLicenses entity = new()
                {
                    LicenseId = licenseId,
                    UserId = applicationUserId
                };
                _context.ApplicationUserLicenses.Add(entity);
                await _unitOfWork.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new NoxLicenseInfrastructureException(ex);
            }
        }

        #endregion
    }
}