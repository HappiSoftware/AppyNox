using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppyNox.Services.License.SharedEvents.Events
{
    public class AssignLicenseToUser
    {
        #region Properties

        public Guid CorrelationId { get; set; }

        public Guid UserId { get; set; }

        public string? LicenseKey { get; set; }

        #endregion
    }
}