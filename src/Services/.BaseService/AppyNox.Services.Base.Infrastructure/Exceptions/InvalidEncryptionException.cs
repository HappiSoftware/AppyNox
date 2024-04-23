using AppyNox.Services.Base.Infrastructure.Exceptions.Base;
using System.Net;

namespace AppyNox.Services.Base.Infrastructure.Exceptions;

internal class InvalidEncryptionException(string message)
        : NoxInfrastructureException(
            message,
            (int)NoxInfrastructureExceptionCode.InvalidEncryptionSettingError,
            (int)HttpStatusCode.InternalServerError)
{
}