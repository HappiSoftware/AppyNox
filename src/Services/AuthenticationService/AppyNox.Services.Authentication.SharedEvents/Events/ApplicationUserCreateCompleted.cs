using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppyNox.Services.Authentication.SharedEvents.Events
{
    public class ApplicationUserCreateCompleted
    {
        #region Properties

        public Guid CorrelationId { get; }

        public Guid UserId { get; }

        #endregion
    }
}