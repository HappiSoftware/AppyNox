using AppyNox.Services.Base.Application.Exceptions.Base;
using AppyNox.Services.Base.Application.Localization;
using AppyNox.Services.Base.Core.Common;
using AppyNox.Services.Base.Core.Enums;
using AppyNox.Services.Base.Core.Extensions;

namespace AppyNox.Services.Base.Application.Exceptions;

/// <summary>
/// Exception thrown when no validator is found for a given DTO type.
/// </summary>
internal class ValidatorNotFoundException(Type dtoType)
    : NoxApplicationException(
        NoxApplicationResourceService.ValidatorNotFound.Format(dtoType),
        (int)NoxApplicationExceptionCode.ValidatorNotFound)
{
}