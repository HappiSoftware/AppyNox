using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppyNox.Services.Base.Infrastructure.Helpers
{
    public static class CorrelationContext
    {
        #region [ Fields ]

        private static readonly AsyncLocal<string> _correlationId = new();

        #endregion

        #region [ Properties ]

        public static string CorrelationId
        {
            get => _correlationId.Value;
            set => _correlationId.Value = value;
        }

        #endregion
    }
}