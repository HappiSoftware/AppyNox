using AppyNox.Services.Base.Domain.ExceptionExtensions.Base;
using AutoWrapper;
using AutoWrapper.Wrappers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppyNox.Services.Base.API.ExceptionExtensions
{
    public class NoxApiValidationExceptionWrapObject(NoxException error, string correlationId, IEnumerable<ValidationError> validationErrors)
        : NoxApiExceptionWrapObject(error, correlationId)
    {
        #region [ Properties ]

        [AutoWrapperPropertyMap(Prop.ResponseException)]
        public new object Error { get; set; } = new NoxError(error, correlationId, validationErrors);

        #endregion

        #region [ Classes ]

        protected new class NoxError(NoxException error, string correlationId, IEnumerable<ValidationError> validationErrors)
            : NoxApiExceptionWrapObject.NoxError(error, correlationId)
        {
            #region [ Properties ]

            public IEnumerable<ValidationError> ValidationErrors { get; set; } = validationErrors;

            #endregion
        }

        #endregion
    }
}