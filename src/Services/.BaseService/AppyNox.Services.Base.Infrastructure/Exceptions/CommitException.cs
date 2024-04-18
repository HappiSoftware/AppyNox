using AppyNox.Services.Base.Infrastructure.Exceptions.Base;
using AppyNox.Services.Base.Infrastructure.Localization;
using System.Net;

namespace AppyNox.Services.Base.Infrastructure.Exceptions;

internal class CommitException(Exception ex)
        : NoxInfrastructureException(
            NoxInfrastructureResourceService.CommitException,
            (int)NoxInfrastructureExceptionCode.CommitError,
            (int)HttpStatusCode.InternalServerError,
            ex)
{
}