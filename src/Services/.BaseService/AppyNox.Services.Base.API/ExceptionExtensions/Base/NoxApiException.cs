using AppyNox.Services.Base.Application.Interfaces.Exceptions;
using AppyNox.Services.Base.Core.Enums;
using AppyNox.Services.Base.Core.ExceptionExtensions.Base;

namespace AppyNox.Services.Base.API.ExceptionExtensions.Base
{
    #region [ NoxApiException Code ]

    internal enum NoxApiExceptionCode
    {
        DevelopmentException = 500,

        CorrelationIdError = 1000,

        AuthenticationInvalidToken = 1001,

        AuthenticationNullToken = 1002,

        ExpiredToken = 1003,

        AuthorizationFailed = 1004,

        AuthorizationInvalidToken = 1005,
    }

    #endregion

    /// <summary>
    /// Represents exceptions specific to the API layer of the api.
    /// </summary>
    public class NoxApiException : NoxException, INoxApiException
    {
        #region [ Public Constructors ]

        /// <summary>
        /// Initializes a new instance of the <see cref="NoxApiException"/> class with a specific error message.
        /// Http status code is set to 500.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        /// <param name="exceptionCode">The code of the exception.</param>
        /// <param name="service">The service of the exception, representing the service where the exception is thrown.</param>
        public NoxApiException(string message, int exceptionCode, string service = "Base")
            : base(ExceptionThrownLayer.Api, service, message, exceptionCode)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="NoxApiException"/> class with a specific error message and status code.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        /// <param name="exceptionCode">The code of the exception.</param>
        /// <param name="statusCode">The HTTP status code associated with the exception.</param>
        /// <param name="service">The service of the exception, representing the service where the exception is thrown.</param>
        public NoxApiException(string message, int exceptionCode, int statusCode, string service = "Base")
            : base(ExceptionThrownLayer.Api, service, message, exceptionCode, statusCode)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="NoxApiException"/> class with an inner exception and an optional message.
        /// Http status code is set to 500.
        /// </summary>
        /// <param name="ex">The inner exception.</param>
        /// <param name="exceptionCode">The code of the exception.</param>
        /// <param name="message">The message that describes the error. Defaults to "Unexpected error" if not provided.</param>
        /// <param name="service">The service of the exception, representing the service where the exception is thrown.</param>
        public NoxApiException(Exception ex, int exceptionCode, string message = "Unexpected error", string service = "Base")
            : base(ExceptionThrownLayer.Api, service, message, exceptionCode, ex)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="NoxApiException"/> class with an inner exception and an optional message with StatusCode.
        /// </summary>
        /// <param name="ex">The inner exception.</param>
        /// <param name="message">The message that describes the error.</param>
        /// <param name="statusCode">The HTTP status code associated with the exception.</param>
        /// <param name="exceptionCode">The code of the exception.</param>
        /// <param name="service">The service of the exception, representing the service where the exception is thrown.</param>
        public NoxApiException(Exception ex, string message, int exceptionCode, int statusCode, string service = "Base")
            : base(ExceptionThrownLayer.Api, service, message, exceptionCode, statusCode, ex)
        {
        }

        #endregion
    }
}