using AppyNox.Services.Base.Core.Enums;
using AppyNox.Services.Base.Core.Exceptions.Base;
using System.Net;

namespace AppyNox.Services.Base.Domain.Exceptions.Base;

#region [ NoxDomainException Code]

internal enum NoxDomainExceptionCode
{
    Undefined = 999
}

#endregion

/// <summary>
/// Initializes a new instance of the <see cref="NoxDomainExceptionBase"/> class with a specific error message and 422 status code.
/// </summary>
/// <param name="product">The service of the exception, representing the service where the exception is thrown.</param>
/// <param name="service">The service of the exception, representing the service where the exception is thrown.</param>
/// <param name="message">The message that describes the error.</param>
/// <param name="exceptionCode">The code of the exception.</param>
public abstract class NoxDomainExceptionBase(
    Enum product,
    string service,
    string message,
    int exceptionCode = (int)NoxDomainExceptionCode.Undefined)
    : NoxException(
        product,
        service,
        ExceptionThrownLayer.Domain,
        message,
        exceptionCode,
        (int)HttpStatusCode.Conflict), INoxDomainException
{
    #region [ Fields ]

    #endregion
}

public interface INoxDomainException
{
}