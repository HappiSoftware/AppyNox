using AppyNox.Services.Base.Core.Enums;
using AppyNox.Services.Base.Infrastructure.Exceptions.Base;
using AppyNox.Services.License.Domain;

namespace AppyNox.Services.License.Infrastructure.ExceptionExtensions;

#region [ NoxLicenseInfrastructureException Code]

internal enum NoxLicenseInfrastructureExceptionCode
{
}

#endregion

internal class NoxLicenseInfrastructureException(
    string? message = null,
    int? exceptionCode = null,
    int? statusCode = null,
    Exception? innerException = null)
    : NoxInfrastructureExceptionBase(
        ExceptionProduct.AppyNox,
        LicenseCommonStrings.Service,
        message,
        exceptionCode,
        statusCode,
        innerException)
{
}