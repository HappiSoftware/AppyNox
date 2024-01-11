using AppyNox.Services.Base.Application.Interfaces.Exceptions;
using AppyNox.Services.Base.Infrastructure.ExceptionExtensions.Base;

namespace AppyNox.Services.License.Infrastructure.ExceptionExtensions
{
    internal class NoxLicenseInfrastructureException : NoxInfrastructureException, INoxInfrastructureException
    {
        #region [ Fields ]

        private const string _service = "License";

        #endregion

        #region [ Public Constructors ]

        public NoxLicenseInfrastructureException(string message)
            : base(message, _service)
        {
        }

        public NoxLicenseInfrastructureException(string message, int statusCode)
            : base(message, statusCode, _service)
        {
        }

        public NoxLicenseInfrastructureException(Exception ex, string message = "Unexpected error")
            : base(ex, message, _service)
        {
        }

        #endregion
    }
}