using AppyNox.Services.Base.Application.ExceptionExtensions.Base;
using AppyNox.Services.Base.Application.Interfaces.Exceptions;

namespace AppyNox.Services.Authentication.Application.ExceptionExtensions
{
    internal class NoxAuthenticationApplicationException : NoxApplicationException, INoxApplicationException
    {
        #region [ Fields ]

        private const string _service = "Application";

        #endregion

        #region [ Public Constructors ]

        public NoxAuthenticationApplicationException(string message)
            : base(message, _service)
        {
        }

        public NoxAuthenticationApplicationException(string message, int statusCode)
            : base(message, statusCode, _service)
        {
        }

        public NoxAuthenticationApplicationException(Exception ex, string message = "Unexpected error")
            : base(ex, message, _service)
        {
        }

        public NoxAuthenticationApplicationException(Exception ex, string message, int statusCode)
            : base(ex, message, statusCode, _service)
        {
        }

        #endregion
    }
}