using AppyNox.Services.Base.API.ExceptionExtensions.Base;
using System.Net;

namespace AppyNox.Services.Coupon.WebAPI.ExceptionExtensions.Base;

#region [ NoxSsoApiException Code ]

internal enum NoxCouponApiExceptionCode
{
    CouponApiError = 999
}

#endregion

internal class NoxCouponApiException : NoxApiException
{
    #region [ Fields ]

    private const string _service = "Coupon";

    #endregion

    #region [ Public Constructors ]

    /// <summary>
    /// Initializes a new instance of the <see cref="NoxCouponApiException"/> class with an inner exception and an optional message.
    /// <para>Http status code is set to 500 (Internal Server Error).</para>
    /// <para>See <see cref="NoxApiException"/> for more information.</para>
    /// </summary>
    /// <param name="message">The message that describes the error.</param>
    /// <param name="exceptionCode">The code of the exception.</param>
    public NoxCouponApiException(string message,
                                 int exceptionCode = (int)NoxCouponApiExceptionCode.CouponApiError)
        : base(message, exceptionCode, _service) { }

    /// <summary>
    /// Initializes a new instance of the <see cref="NoxCouponApiException"/> class with a specific error message and status code.
    /// <para><see cref="NoxApiException"/> for more information.</para>
    /// </summary>
    /// <param name="message">The message that describes the error.</param>
    /// <param name="exceptionCode">The code of the exception.</param>
    /// <param name="statusCode">The HTTP status code associated with the exception.</param>
    public NoxCouponApiException(string message,
                                 int exceptionCode = (int)NoxCouponApiExceptionCode.CouponApiError,
                                 int statusCode = (int)HttpStatusCode.InternalServerError)
        : base(message, exceptionCode, statusCode, _service) { }

    /// <summary>
    /// Initializes a new instance of the <see cref="NoxCouponApiException"/> class with an inner exception and an optional message.
    /// <para>Http status code is set to 500 (Internal Server Error).</para>
    /// <para><see cref="NoxApiException"/> for more information.</para>
    /// </summary>
    /// <param name="ex">The inner exception.</param>
    /// <param name="exceptionCode">The code of the exception.</param>
    /// <param name="message">The message that describes the error.</param>
    public NoxCouponApiException(
        Exception ex,
        int exceptionCode = (int)NoxCouponApiExceptionCode.CouponApiError,
        string message = "Unexpected error"
    )
        : base(ex, exceptionCode, message, _service) { }

    /// <summary>
    /// Initializes a new instance of the <see cref="NoxCouponApiException"/> class with an inner exception and an optional message with StatusCode.
    /// </summary>
    /// <para><see cref="NoxApiException"/> for more information.</para>
    /// <param name="ex">The inner exception.</param>
    /// <param name="message">The message that describes the error.</param>
    /// <param name="exceptionCode">The code of the exception.</param>
    /// <param name="statusCode">The HTTP status code associated with the exception.</param>
    public NoxCouponApiException(
        Exception ex,
        string message,
        int exceptionCode = (int)NoxCouponApiExceptionCode.CouponApiError,
        int statusCode = (int)HttpStatusCode.InternalServerError)
        : base(ex, message, exceptionCode, statusCode, _service) { }

    #endregion
}