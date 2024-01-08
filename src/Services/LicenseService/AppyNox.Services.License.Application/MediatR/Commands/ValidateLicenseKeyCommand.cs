using MediatR;

namespace AppyNox.Services.License.Application.MediatR.Commands
{
    public class ValidateLicenseKeyCommand(string licenseKey) : IRequest<bool>
    {
        #region [ Properties ]

        public string LicenseKey { get; set; } = licenseKey;

        #endregion
    }
}