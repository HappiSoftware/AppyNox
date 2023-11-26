using AppyNox.Services.Base.Application.ExceptionExtensions.Base;
using AppyNox.Services.Base.Domain.Common.HttpStatusCodes;
using AppyNox.Services.Base.Domain.ExceptionExtensions.Base;
using AppyNox.Services.Base.Domain.ExceptionExtensions.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace AppyNox.Services.Base.Application.ExceptionExtensions
{
    internal class AccessTypeNotFoundException : NoxApplicationException
    {
        #region [ Internal Constructors ]

        internal AccessTypeNotFoundException(Type entity)
            : base($"This '{entity.FullName}' entity has no access level mapping.", (int)NoxServerErrorResponseCodes.InternalServerError)
        {
        }

        internal AccessTypeNotFoundException(Type entity, string accessType)
            : base($"This '{entity.FullName}' entity has no access level mapping for '{accessType}'.", (int)NoxClientErrorResponseCodes.BadRequest)
        {
        }

        internal AccessTypeNotFoundException(string accessType)
            : base($"'{accessType}' is not a valid Access modifier.", (int)NoxClientErrorResponseCodes.BadRequest)
        {
        }

        #endregion
    }
}