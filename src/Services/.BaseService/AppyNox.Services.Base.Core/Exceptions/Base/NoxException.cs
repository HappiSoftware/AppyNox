using AppyNox.Services.Base.Core.AsyncLocals;
using AppyNox.Services.Base.Core.Common;
using AppyNox.Services.Base.Core.Extensions;
using System.Net;
using System.Text.Json.Serialization;

namespace AppyNox.Services.Base.Core.Exceptions.Base;

/// <summary>
/// Represents a base class for custom exceptions in the application.
/// <para>It is not suggested to use this exception directly in microservices. Please check the documentation for more information.</para>
/// </summary>
/// <remarks>
/// Initializes a new instance of the <see cref="NoxException"/> class with a layer, a specified error message, and a reference to the inner exception that is the cause of this exception.
/// </remarks>
/// <param name="product">The product of the exception, representing the service where the exception is thrown.</param>
/// <param name="service">The service of the exception, representing the service where the exception is thrown.</param>
/// <param name="layer">The layer of the exception, representing the layer where the exception is thrown.</param>
/// <param name="message">The error message that explains the reason for the exception.</param>
/// <param name="exceptionCode">The code of the associated exception.</param>
/// <param name="innerException">The exception that is the cause of the current exception.</param>
public abstract class NoxException(
    Enum product,
    string service,
    Enum layer,
    string? message = null,
    int? exceptionCode = null,
    int? statusCode = null,
    Exception? innerException = null) : Exception(message ?? NoxExceptionStrings.EmptyMessage, innerException), INoxException
{
    #region [ Fields ]

    private readonly string _product = product.GetDisplayName();

    private readonly string _service = service;

    private readonly string _layer = layer.GetDisplayName();

    private readonly int _exceptionCode = exceptionCode ?? 999;

    private readonly int _statusCode = statusCode ?? (int)HttpStatusCode.InternalServerError;

    private readonly Guid _correlationId = NoxContext.CorrelationId;

    #endregion

    #region [ Properties ]

    /// <summary>
    /// Gets the Product of the exception, typically representing the service where the exception is thrown. Ex: Nox or Fleet
    /// </summary>
    public string Product => _product;

    /// <summary>
    /// Gets the Service of the exception, typically representing the service where the exception is thrown. Ex: Sso or Coupon
    /// </summary>
    public string Service => _service;

    /// <summary>
    /// Gets the layer of the exception, typically representing the layer where the exception is thrown. Ex: Application or Infrastructure
    /// </summary>
    public string Layer => _layer;

    /// <summary>
    /// Gets the exception code of the exception
    /// </summary>
    public int ExceptionCode => _exceptionCode;

    /// <summary>
    /// Gets the HTTP status code associated with the exception.
    /// </summary>
    [JsonIgnore]
    public int StatusCode => _statusCode;

    /// <summary>
    /// Gets the correlation id of the request
    /// </summary>
    public Guid CorrelationId => _correlationId;

    #endregion
}