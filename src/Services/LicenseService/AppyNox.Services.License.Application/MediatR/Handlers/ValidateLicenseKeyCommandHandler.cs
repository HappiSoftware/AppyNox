using AppyNox.Services.License.Application.Interfaces;
using AppyNox.Services.License.Application.MediatR.Commands;
using MediatR;

namespace AppyNox.Services.License.Application.MediatR.Handlers
{
    public class ValidateLicenseKeyCommandHandler(ILicenseRepository repository)
        : IRequestHandler<ValidateLicenseKeyCommand, (bool isValid, Guid? CompanyId, Guid? LicenseId)>
    {
        #region [ Fields ]

        private readonly ILicenseRepository _repository = repository;

        #endregion

        #region [ Public Methods ]

        public async Task<(bool isValid, Guid? CompanyId, Guid? LicenseId)> Handle(ValidateLicenseKeyCommand request, CancellationToken cancellationToken)
        {
            var license = await _repository.FindLicenseByKey(request.LicenseKey, cancellationToken);

            if (license == null) return (false, null, null);

            // Check MaxUsers constraint
            var userCount = await _repository.GetUserCountForLicenseKey(license.Id, cancellationToken);

            return (userCount < license.MaxUsers, license.CompanyId, license.Id);
        }

        #endregion
    }
}