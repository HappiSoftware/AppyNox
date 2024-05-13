using AppyNox.Services.Base.Application.Interfaces.Exceptions;
using AppyNox.Services.Base.Core.Common;
using AppyNox.Services.Base.Core.Enums;
using AppyNox.Services.Base.Core.Exceptions.Base;

namespace AppyNox.Services.Base.API.Exceptions.Base;

#region [ NoxApiException Code ]

internal enum NoxApiExceptionCode
{
    DevelopmentException = 500,

    Undefined = 999,

    CorrelationIdError = 1000,

    SwaggerGenerationException = 1001
}

#endregion

/// <summary>
/// Provides a constructor for derived exception classes in the api layer, ensuring all
/// api-related exceptions are standardized.
/// </summary>
/// <remarks>
/// This constructor is part of an abstract class and is intended to be invoked by constructors of derived classes.
/// It sets initial values for exception properties, with defaults provided for optional parameters.
/// This standardization helps in maintaining consistency across all exceptions thrown within the api layer.
/// <para>
/// Example of using this constructor in a derived class:
/// <code>
/// internal class NoxCouponApiException(
///     string? message = null,
///     int? exceptionCode = null,
///     int? statusCode = null,
///     Exception? innerException = null)
///     : NoxApiExceptionBase(
///         ExceptionProduct.AppyNox,       // DisplayName of the enum: "Nox"
///         NoxCouponCommonStrings.Service, // "Coupon"
///         message,
///         exceptionCode,
///         statusCode,
///         innerException)
/// {
/// }
/// </code>
/// </para>
/// </remarks>
/// <param name="product">The product associated with the exception, representing the high-level category of the service
/// or component where the exception originated.</param>
///
/// <param name="service">The specific service within the product that the exception pertains to.</param>
///
/// <param name="message">The message that describes the error. This message is optional; a default message may be used
/// if not provided (A default error message was not provided.).</param>
///
/// <param name="exceptionCode">The custom code identifying the type of exception. This is optional and a default may be
/// defined if not provided (999).</param>
///
/// <param name="statusCode">The HTTP status code associated with the exception. This is optional; a default status code
/// may be used if not provided (500).</param>
///
/// <param name="innerException">The inner exception that is the cause of this exception. This is optional and can be null
/// if there is no inner exception.</param>
public abstract class NoxApiExceptionBase(
    Enum product,
    string service,
    string? message = null,
    int? exceptionCode = null,
    int? statusCode = null,
    Exception? innerException = null)
    : NoxException(
        product,
        service,
        ExceptionThrownLayer.Api,
        message,
        exceptionCode,
        statusCode,
        innerException), INoxApiException
{
}

internal class NoxApiException(
    string? message = null,
    int? exceptionCode = null,
    int? statusCode = null,
    Exception? innerException = null)
    : NoxApiExceptionBase(
        ExceptionProduct.AppyNox,
        NoxExceptionStrings.Base,
        message,
        exceptionCode,
        statusCode,
        innerException)
{
}

internal class NoxExceptionClone(NoxException exception, Exception innerException)
: NoxApiExceptionBase(
        (ExceptionProduct)Enum.Parse(typeof(ExceptionProduct), exception.Product), 
        // Important, we do not expect a constructed NoxException have Product property something else than ExceptionProduct 
        exception.Service,
        exception.Message,
        exception.ExceptionCode,
        exception.StatusCode,
        innerException)
{
}