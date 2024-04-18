using AppyNox.Services.Base.Core.Exceptions.Base;
using AppyNox.Services.License.Application.Exceptions;
using AppyNox.Services.License.Application.Interfaces;
using AppyNox.Services.License.Application.MediatR.Commands;
using MediatR;

namespace AppyNox.Services.License.Application.MediatR.Handlers;

internal sealed class AssignLicenseKeyToApplicationUserCommandHandler(ILicenseRepository repository)
        : IRequestHandler<AssignLicenseKeyToApplicationUserCommand>
{
    #region [ Fields ]

    private readonly ILicenseRepository _licenseRepository = repository;

    #endregion

    #region [ Public Methods ]

    public async Task Handle(AssignLicenseKeyToApplicationUserCommand request, CancellationToken cancellationToken)
    {
        try
        {
            await _licenseRepository.AssignLicenseToApplicationUserAsync(request.LicenseId, request.UserId);
        }
        catch (Exception ex) when (ex is INoxException)
        {
            throw;
        }
        catch (Exception ex)
        {
            throw new NoxLicenseApplicationException(exceptionCode: (int)NoxLicenseApplicationExceptionCode.AssignKeyCommandError, innerException: ex);
        }
    }

    #endregion
}