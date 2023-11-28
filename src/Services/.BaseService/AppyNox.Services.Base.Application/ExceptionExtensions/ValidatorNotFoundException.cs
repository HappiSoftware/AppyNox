using AppyNox.Services.Base.Application.ExceptionExtensions.Base;
using AppyNox.Services.Base.Domain.Common.HttpStatusCodes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppyNox.Services.Base.Application.ExceptionExtensions
{
    internal class ValidatorNotFoundException : NoxApplicationException
    {
        #region Internal Constructors

        internal ValidatorNotFoundException(Type dtoType)
            : base($"No validator found for '{dtoType}'.", (int)NoxServerErrorResponseCodes.InternalServerError)
        {
        }

        #endregion
    }
}