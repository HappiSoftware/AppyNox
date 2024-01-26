using AppyNox.Services.Base.Application.ExceptionExtensions.Base;
using AppyNox.Services.Base.Application.Interfaces.Exceptions;
using AppyNox.Services.Base.Application.Localization;

namespace AppyNox.Services.License.Application.ExceptionExtensions
{
    internal class NoxLicenseApplicationException : NoxApplicationException, INoxApplicationException
    {
        #region [ Fields ]

        private const string _service = "License";

        #endregion

        #region [ Public Constructors ]

        public NoxLicenseApplicationException(string message)
            : base(message, _service)
        {
        }

        public NoxLicenseApplicationException(string message, int statusCode)
            : base(message, statusCode, _service)
        {
        }

        public NoxLicenseApplicationException(Exception ex, string? message = null)
            : base(ex, message ?? NoxApplicationResourceService.UnexpectedError, _service)
        {
        }

        public NoxLicenseApplicationException(Exception ex, string message, int statusCode)
            : base(ex, message, statusCode, _service)
        {
        }

        #endregion
    }
}