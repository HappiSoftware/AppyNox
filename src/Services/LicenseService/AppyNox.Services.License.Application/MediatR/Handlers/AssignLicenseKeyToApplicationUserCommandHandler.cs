using AppyNox.Services.License.Application.Interfaces;
using AppyNox.Services.License.Application.MediatR.Commands;
using MediatR;

namespace AppyNox.Services.License.Application.MediatR.Handlers
{
    internal sealed class AssignLicenseKeyToApplicationUserCommandHandler(ILicenseRepository repository)
        : IRequestHandler<AssignLicenseKeyToApplicationUserCommand>
    {
        #region [ Fields ]

        private readonly ILicenseRepository _licenseRepository = repository;

        #endregion

        #region [ Public Methods ]

        public async Task Handle(AssignLicenseKeyToApplicationUserCommand request, CancellationToken cancellationToken)
        {
            await _licenseRepository.AssignLicenseToApplicationUser(request.LicenseId, request.UserId);
        }

        #endregion
    }
}