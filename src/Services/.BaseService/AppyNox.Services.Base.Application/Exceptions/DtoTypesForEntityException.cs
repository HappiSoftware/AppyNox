using AppyNox.Services.Base.Application.Exceptions.Base;

namespace AppyNox.Services.Base.Application.Exceptions;

internal class DtoTypesForEntityException(string message)
    : NoxApplicationException(message, (int)NoxApplicationExceptionCode.DtoTypesForEntityMethodError)
{
}