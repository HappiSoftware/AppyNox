using AppyNox.Services.Base.Application.Interfaces.Exceptions;
using AppyNox.Services.Base.Core.Common;
using AppyNox.Services.Base.Core.Enums;
using AppyNox.Services.Base.Core.Exceptions.Base;

namespace AppyNox.Services.Base.Infrastructure.Exceptions.Base;

#region [ NoxInfrastructureException Code ]

internal enum NoxInfrastructureExceptionCode
{
    DevelopmentError = 500,

    CommitError = 1000,

    WrongIdError = 1001,

    MultipleDataFetchingError = 1002,

    DataFetchingError = 1003,

    AddingDataError = 1004,

    UpdatingDataError = 1005,

    DeletingDataError = 1006,

    ProjectionError = 1007,

    QueryParameterError = 1008,

    SqlInjectionError = 1009,

    InvalidEncryptionSettingError = 1010,

    AuthenticationInvalidToken = 1011,

    AuthenticationNullToken = 1012,

    ExpiredToken = 1013,

    AuthorizationFailed = 1014,

    AuthorizationInvalidToken = 1015

}

#endregion

/// <summary>
/// Provides a constructor for derived exception classes in the infrastructure layer, ensuring all
/// infrastructure-related exceptions are standardized.
/// </summary>
/// <remarks>
/// This constructor is part of an abstract class and is intended to be invoked by constructors of derived classes.
/// It sets initial values for exception properties, with defaults provided for optional parameters.
/// This standardization helps in maintaining consistency across all exceptions thrown within the infrastructure layer.
/// <para>
/// Example of using this constructor in a derived class:
/// <code>
/// internal class NoxCouponInfrastructureException(
///     string? message = null,
///     int? exceptionCode = null,
///     int? statusCode = null,
///     Exception? innerException = null)
///     : NoxInfrastructureExceptionBase(
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
public abstract class NoxInfrastructureExceptionBase(
    Enum product,
    string service,
    string? message = null,
    int? exceptionCode = null,
    int? statusCode = null,
    Exception? innerException = null)
    : NoxException(
        product,
        service,
        ExceptionThrownLayer.Infrastructure,
        message,
        exceptionCode,
        statusCode,
        innerException), INoxInfrastructureException
{
}

internal class NoxInfrastructureException(
    string? message = null,
    int? exceptionCode = null,
    int? statusCode = null,
    Exception? innerException = null)
    : NoxInfrastructureExceptionBase(
        ExceptionProduct.AppyNox,
        NoxExceptionStrings.Base,
        message,
        exceptionCode,
        statusCode,
        innerException)
{
}