using AppyNox.Services.Base.Application.Interfaces.Exceptions;
using AppyNox.Services.Base.Domain.ExceptionExtensions.Base;
using AppyNox.Services.Base.Domain.ExceptionExtensions.Enums;

namespace AppyNox.Services.Base.Application.ExceptionExtensions.Base
{
    /// <summary>
    /// Represents exceptions specific to the application layer of the application.
    /// </summary>
    public class NoxApplicationException : NoxException, INoxApplicationException
    {
        #region [ Public Constructors ]

        /// <summary>
        /// Initializes a new instance of the <see cref="NoxApplicationException"/> class with a specific error message.
        /// Http status code is set to 500.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        public NoxApplicationException(string message, string service = "Base")
            : base(ExceptionThrownLayer.Application, service, message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="NoxApplicationException"/> class with a specific error message and status code.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        /// <param name="statusCode">The HTTP status code associated with the exception.</param>
        public NoxApplicationException(string message, int statusCode, string service = "Base")
            : base(ExceptionThrownLayer.Application, service, message, statusCode)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="NoxApplicationException"/> class with an inner exception and an optional message.
        /// Http status code is set to 500.
        /// </summary>
        /// <param name="ex">The inner exception.</param>
        /// <param name="message">The message that describes the error. Defaults to "Unexpected error" if not provided.</param>
        public NoxApplicationException(Exception ex, string message = "Unexpected error", string service = "Base")
            : base(ExceptionThrownLayer.Application, service, message, ex)
        {
        }

        #endregion
    }
}