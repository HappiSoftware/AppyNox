using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppyNox.Services.License.SharedEvents.Events
{
    public class LicenseValidationCompleted
    {
        #region Properties

        public Guid CorrelationId { get; }

        public bool IsValid { get; }

        #endregion
    }
}