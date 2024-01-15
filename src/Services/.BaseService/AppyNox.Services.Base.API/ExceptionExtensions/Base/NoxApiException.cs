using AppyNox.Services.Base.Application.Interfaces.Exceptions;
using AppyNox.Services.Base.Core.Enums;
using AppyNox.Services.Base.Core.ExceptionExtensions.Base;

namespace AppyNox.Services.Base.API.ExceptionExtensions.Base
{
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
        public NoxApiException(string message, string service = "Base")
            : base(ExceptionThrownLayer.Api, service, message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="NoxApiException"/> class with a specific error message and status code.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        /// <param name="statusCode">The HTTP status code associated with the exception.</param>
        public NoxApiException(string message, int statusCode, string service = "Base")
            : base(ExceptionThrownLayer.Api, service, message, statusCode)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="NoxApiException"/> class with an inner exception and an optional message.
        /// Http status code is set to 500.
        /// </summary>
        /// <param name="ex">The inner exception.</param>
        /// <param name="message">The message that describes the error. Defaults to "Unexpected error" if not provided.</param>
        public NoxApiException(Exception ex, string message = "Unexpected error", string service = "Base")
            : base(ExceptionThrownLayer.Api, service, message, ex)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="NoxApiException"/> class with an inner exception and an optional message with StatusCode.
        /// </summary>
        /// <param name="ex">The inner exception.</param>
        /// <param name="message">The message that describes the error.</param>
        /// <param name="statusCode">The HTTP status code associated with the exception.</param>
        /// <param name="service">The service of the exception, representing the service where the exception is thrown.</param>
        public NoxApiException(Exception ex, string message, int statusCode, string service = "Base")
            : base(ExceptionThrownLayer.Api, service, message, statusCode, ex)
        {
        }

        #endregion
    }
}