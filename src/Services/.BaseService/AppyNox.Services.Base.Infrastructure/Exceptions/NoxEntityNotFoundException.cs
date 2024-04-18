using AppyNox.Services.Base.Core.Common;
using AppyNox.Services.Base.Core.Enums;
using AppyNox.Services.Base.Core.Extensions;
using AppyNox.Services.Base.Infrastructure.Exceptions.Base;
using AppyNox.Services.Base.Infrastructure.Localization;
using System.Net;

namespace AppyNox.Services.Base.Infrastructure.Exceptions;

public class NoxEntityNotFoundException<TEntity>(Guid entityId)
        : NoxInfrastructureExceptionBase(
            ExceptionProduct.AppyNox,
            NoxExceptionStrings.Base,
            message: NoxInfrastructureResourceService.EntityNotFound.Format(typeof(TEntity).Name, entityId),
            exceptionCode: (int)NoxInfrastructureExceptionCode.WrongIdError,
            statusCode: (int)HttpStatusCode.NotFound)
{
}