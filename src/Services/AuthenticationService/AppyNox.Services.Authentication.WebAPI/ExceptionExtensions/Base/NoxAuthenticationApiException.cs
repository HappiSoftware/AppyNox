using AppyNox.Services.Base.API.ExceptionExtensions.Base;

namespace AppyNox.Services.Authentication.WebAPI.ExceptionExtensions.Base
{
    /// <summary>
    /// Exception type for handling authentication-related errors in Api layer.
    /// </summary>
    public class NoxAuthenticationApiException : NoxApiException
    {
        #region [ Fields ]

        private const string _service = "Authentication";

        #endregion

        #region [ Public Constructors ]

        public NoxAuthenticationApiException(string message)
            : base(message, _service)
        {
        }

        public NoxAuthenticationApiException(string message, int statusCode)
            : base(message, statusCode, _service)
        {
        }

        public NoxAuthenticationApiException(Exception ex, string message = "Unexpected error")
            : base(ex, message, _service)
        {
        }

        public NoxAuthenticationApiException(Exception ex, string message, int statusCode)
            : base(ex, message, statusCode, _service)
        {
        }

        #endregion
    }
}