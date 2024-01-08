using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppyNox.Services.Authentication.SharedEvents.Events
{
    public class StartUserCreation
    {
        #region Properties

        public Guid CorrelationId { get; set; }

        public string LicenseKey { get; set; } = string.Empty;

        public string UserName { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;

        public string Password { get; set; } = string.Empty;

        public string ConfirmPassword { get; set; } = string.Empty;

        #endregion
    }
}