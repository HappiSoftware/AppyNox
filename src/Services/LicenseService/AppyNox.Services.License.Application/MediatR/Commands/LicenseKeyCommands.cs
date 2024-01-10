using MediatR;

namespace AppyNox.Services.License.Application.MediatR.Commands
{
    public record ValidateLicenseKeyCommand(string LicenseKey) : IRequest<(bool isValid, Guid? companyId, Guid? licenseId)>;
    public record AssignLicenseKeyToApplicationUserCommand(Guid LicenseId, Guid UserId) : IRequest;
}