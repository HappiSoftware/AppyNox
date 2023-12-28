using AppyNox.Services.Base.Domain.ExceptionExtensions.Base;
using AppyNox.Services.Base.Domain.ExceptionExtensions.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppyNox.Services.Base.Infrastructure.ExceptionExtensions.Base
{
    public class NoxInfrastructureException : NoxException
    {
        #region [ Public Constructors ]

        public NoxInfrastructureException(string message, int statusCode)
        : base(ExceptionThrownLayer.InfrastructureBase, message, statusCode)
        {
        }

        public NoxInfrastructureException(Exception ex, string message = "Unexpected error")
            : base(ExceptionThrownLayer.InfrastructureBase, message, ex)
        {
        }

        #endregion
    }
}