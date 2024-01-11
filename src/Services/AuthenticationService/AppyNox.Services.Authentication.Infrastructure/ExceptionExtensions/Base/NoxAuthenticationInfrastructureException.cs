using AppyNox.Services.Base.Application.Interfaces.Exceptions;
using AppyNox.Services.Base.Infrastructure.ExceptionExtensions.Base;

namespace AppyNox.Services.Authentication.Infrastructure.ExceptionExtensions.Base
{
    internal class NoxAuthenticationInfrastructureException : NoxInfrastructureException, INoxInfrastructureException
    {
        #region [ Fields ]

        private const string _service = "Authentication";

        #endregion

        #region [ Public Constructors ]

        /// <summary>
        /// Initializes a new instance of the <see cref="NoxAuthenticationInfrastructureException"/> class with an inner exception and an optional message.
        /// <para>Http status code is set to 500 (Internal Server Error).</para>
        /// <para>See <see cref="NoxInfrastructureException"/> for more information.</para>
        /// </summary>
        /// <param name="ex">The inner exception.</param>
        /// <param name="message">The message that describes the error.</param>
        public NoxAuthenticationInfrastructureException(string message)
            : base(message, _service)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="NoxAuthenticationInfrastructureException"/> class with a specific error message and status code.
        /// <para><see cref="NoxInfrastructureException"/> for more information.</para>
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        /// <param name="statusCode">The HTTP status code associated with the exception.</param>
        public NoxAuthenticationInfrastructureException(string message, int statusCode)
            : base(message, statusCode, _service)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="NoxAuthenticationInfrastructureException"/> class with an inner exception and an optional message.
        /// <para>Http status code is set to 500 (Internal Server Error).</para>
        /// <para><see cref="NoxInfrastructureException"/> for more information.</para>
        /// </summary>
        /// <param name="ex">The inner exception.</param>
        /// <param name="message">The message that describes the error.</param>
        public NoxAuthenticationInfrastructureException(Exception ex, string message = "Unexpected error")
            : base(ex, message, _service)
        {
        }

        #endregion
    }
}