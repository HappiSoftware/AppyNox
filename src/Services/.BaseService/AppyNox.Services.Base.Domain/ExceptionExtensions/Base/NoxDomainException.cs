using AppyNox.Services.Base.Core.Enums;
using AppyNox.Services.Base.Core.ExceptionExtensions.Base;
using System.Net;

namespace AppyNox.Services.Base.Domain.ExceptionExtensions.Base;

#region [ NoxDomainException Code]

internal enum NoxDomainExceptionCode
{
    AuditableDataValidation = 1000,
}

#endregion

/// <summary>
/// Initializes a new instance of the <see cref="NoxDomainException"/> class with a specific error message and 422 status code.
/// </summary>
/// <param name="message">The message that describes the error.</param>
/// <param name="exceptionCode">The code of the exception.</param>
/// <param name="service">The service of the exception, representing the service where the exception is thrown.</param>
public class NoxDomainException(string message, int exceptionCode, string service = "Base")
    : NoxException(ExceptionThrownLayer.Domain, service, message, exceptionCode, _statusCode), INoxDomainException
{
    #region [ Fields ]

    private static readonly int _statusCode = (int)HttpStatusCode.UnprocessableContent;

    #endregion
}

public interface INoxDomainException
{
}