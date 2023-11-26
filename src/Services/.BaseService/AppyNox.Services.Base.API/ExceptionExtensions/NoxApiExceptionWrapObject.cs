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
    public class NoxApiExceptionWrapObject(NoxException error, string correlationId)
    {
        #region Properties

        [AutoWrapperPropertyMap(Prop.ResponseException)]
        public object Error { get; set; } = new NoxError(error, correlationId);

        #endregion

        #region Classes

        protected class NoxError(NoxException error, string correlationId)
        {
            #region Properties

            public int Code { get; set; } = error.StatusCode;

            public string Title { get; set; } = error.Title;

            public string Message { get; set; } = error.Message;

            public string CorrelationId { get; set; } = correlationId;

            #endregion
        }

        #endregion
    }
}