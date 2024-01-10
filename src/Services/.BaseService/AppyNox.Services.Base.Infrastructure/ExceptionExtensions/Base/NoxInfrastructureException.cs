using AppyNox.Services.Base.Application.Interfaces.Exceptions;
using AppyNox.Services.Base.Domain.ExceptionExtensions.Base;
using AppyNox.Services.Base.Domain.ExceptionExtensions.Enums;

namespace AppyNox.Services.Base.Infrastructure.ExceptionExtensions.Base
{
    /// <summary>
    /// Represents exceptions specific to the infrastructure layer of the application.
    /// </summary>
    public class NoxInfrastructureException : NoxException, INoxInfrastructureException
    {
        #region [ Public Constructors ]

        /// <summary>
        /// Initializes a new instance of the <see cref="NoxInfrastructureException"/> class with a specific error message.
        /// HTTP status code is set to 500 (Internal Server Error).
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        /// <param name="service">The service of the exception, representing the service where the exception is thrown.</param>
        public NoxInfrastructureException(string message, string service = "Base")
        : base(ExceptionThrownLayer.Infrastructure, service, message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="NoxInfrastructureException"/> class with a specific error message and status code.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        /// <param name="statusCode">The HTTP status code associated with the exception.</param>
        /// <param name="service">The service of the exception, representing the service where the exception is thrown.</param>
        public NoxInfrastructureException(string message, int statusCode, string service = "Base")
        : base(ExceptionThrownLayer.Infrastructure, service, message, statusCode)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="NoxInfrastructureException"/> class with an inner exception and an optional message.
        /// Http status code is set to 500 (Internal Server Error).
        /// </summary>
        /// <param name="ex">The inner exception.</param>
        /// <param name="message">The message that describes the error.</param>
        /// <param name="service">The service of the exception, representing the service where the exception is thrown.</param>
        public NoxInfrastructureException(Exception ex, string message = "Unexpected error", string service = "Base")
            : base(ExceptionThrownLayer.Infrastructure, service, message, ex)
        {
        }

        #endregion
    }
}