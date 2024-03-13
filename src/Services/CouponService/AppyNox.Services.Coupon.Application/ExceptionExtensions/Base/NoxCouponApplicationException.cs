using AppyNox.Services.Base.Application.ExceptionExtensions.Base;
using System.Net;

namespace AppyNox.Services.Coupon.Application.ExceptionExtensions.Base;

#region [ NoxSsoApiException Code ]

internal enum NoxCouponApplicationExceptionCode
{
    CouponApplicationError = 999,

    IdsMismatch = 1000,

    UnexpectedUpdateCommandError = 1001,

    UnexpectedDomainEventHandlerError = 1002
}

#endregion

internal class NoxCouponApplicationException
    : NoxApplicationException
{
    #region Fields

    private const string _service = "Coupon";

    #endregion

    #region [ Internal Constructors ]

    /// <summary>
    /// Initializes a new instance of the <see cref="NoxCouponApplicationException"/> class with an inner exception and an optional message.
    /// <para>Http status code is set to 500 (Internal Server Error).</para>
    /// <para>See <see cref="NoxApiException"/> for more information.</para>
    /// </summary>
    /// <param name="message">The message that describes the error.</param>
    /// <param name="exceptionCode">The code of the exception.</param>
    internal NoxCouponApplicationException(string message,
                                           int exceptionCode = (int)NoxCouponApplicationExceptionCode.CouponApplicationError)
        : base(message, exceptionCode, _service)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="NoxCouponApplicationException"/> class with a specific error message and status code.
    /// <para><see cref="NoxApplicationException"/> for more information.</para>
    /// </summary>
    /// <param name="message">The message that describes the error.</param>
    /// <param name="exceptionCode">The code of the exception.</param>
    /// <param name="statusCode">The HTTP status code associated with the exception.</param>
    internal NoxCouponApplicationException(string message,
                                           int exceptionCode = (int)NoxCouponApplicationExceptionCode.CouponApplicationError,
                                           int statusCode = (int)HttpStatusCode.InternalServerError)
        : base(message, exceptionCode, statusCode, _service)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="NoxCouponApplicationException"/> class with an inner exception and an optional message.
    /// <para>Http status code is set to 500 (Internal Server Error).</para>
    /// <para><see cref="NoxApplicationException"/> for more information.</para>
    /// </summary>
    /// <param name="ex">The inner exception.</param>
    /// <param name="exceptionCode">The code of the exception.</param>
    /// <param name="message">The message that describes the error.</param>
    internal NoxCouponApplicationException(Exception ex,
                                           int exceptionCode = (int)NoxCouponApplicationExceptionCode.CouponApplicationError,
                                           string message = "Unexpected error")
        : base(ex, exceptionCode, message, _service)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="NoxCouponApplicationException"/> class with an inner exception and an optional message with StatusCode.
    /// </summary>
    /// <para><see cref="NoxApplicationException"/> for more information.</para>
    /// <param name="ex">The inner exception.</param>
    /// <param name="message">The message that describes the error.</param>
    /// <param name="exceptionCode">The code of the exception.</param>
    /// <param name="statusCode">The HTTP status code associated with the exception.</param>
    internal NoxCouponApplicationException(Exception ex,
                                           string message,
                                           int exceptionCode = (int)NoxCouponApplicationExceptionCode.CouponApplicationError,
                                           int statusCode = (int)HttpStatusCode.InternalServerError)
        : base(ex, message, exceptionCode, statusCode, _service)
    {
    }

    #endregion
}