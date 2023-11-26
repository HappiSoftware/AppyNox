using AppyNox.Services.Base.Domain.ExceptionExtensions.Base;
using AppyNox.Services.Base.Domain.ExceptionExtensions.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppyNox.Services.Base.Application.ExceptionExtensions.Base
{
    public class NoxApplicationException : NoxException
    {
        #region [ Public Constructors ]

        public NoxApplicationException(string message, int statusCode)
            : base(ExceptionThrownLayer.ApplicationBase, message, statusCode)
        {
        }

        #endregion
    }
}