using AppyNox.Services.Base.Domain.ExceptionExtensions.Base;
using AppyNox.Services.Base.Domain.ExceptionExtensions.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppyNox.Services.Base.API.ExceptionExtensions.Base
{
    internal class NoxApiException : NoxException
    {
        #region Internal Constructors

        internal NoxApiException(string message, int statusCode)
            : base(ExceptionThrownLayer.ApiBase, message, statusCode)
        {
        }

        #endregion
    }
}