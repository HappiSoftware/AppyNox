using AppyNox.Services.Base.Domain.Common.HttpStatusCodes;
using AppyNox.Services.Base.Domain.ExceptionExtensions.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppyNox.Services.Base.Domain.ExceptionExtensions
{
    internal class EnumDisplayNameNotFoundException : NoxException
    {
        #region [ Internal Constructors ]

        internal EnumDisplayNameNotFoundException(Enum enumValue)
            : base($"DisplayName not found for enum '{enumValue}'", (int)NoxServerErrorResponseCodes.InternalServerError)
        {
        }

        #endregion
    }
}