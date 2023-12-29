using AppyNox.Services.Base.Domain.ExceptionExtensions.Base;
using AppyNox.Services.Base.Domain.ExceptionExtensions.Enums;

namespace AppyNox.Services.Base.Infrastructure.ExceptionExtensions.Base
{
    /// <summary>
    /// Represents exceptions specific to the infrastructure layer of the application.
    /// </summary>
    public class NoxInfrastructureException : NoxException
    {
        #region [ Public Constructors ]

        /// <summary>
        /// Initializes a new instance of the <see cref="NoxInfrastructureException"/> class with a specific error message.
        /// HTTP status code is set to 500 (Internal Server Error).
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        public NoxInfrastructureException(string message)
        : base(ExceptionThrownLayer.InfrastructureBase, message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="NoxInfrastructureException"/> class with a specific error message and status code.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        /// <param name="statusCode">The HTTP status code associated with the exception.</param>
        public NoxInfrastructureException(string message, int statusCode)
        : base(ExceptionThrownLayer.InfrastructureBase, message, statusCode)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="NoxInfrastructureException"/> class with an inner exception and an optional message.
        /// Http status code is set to 500 (Internal Server Error).
        /// </summary>
        /// <param name="ex">The inner exception.</param>
        /// <param name="message">The message that describes the error.</param>
        public NoxInfrastructureException(Exception ex, string message = "Unexpected error")
            : base(ExceptionThrownLayer.InfrastructureBase, message, ex)
        {
        }

        #endregion
    }
}