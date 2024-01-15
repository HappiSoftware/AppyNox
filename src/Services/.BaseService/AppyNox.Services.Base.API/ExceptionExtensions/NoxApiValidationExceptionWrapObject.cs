using AppyNox.Services.Base.Core.ExceptionExtensions.Base;
using AutoWrapper;
using AutoWrapper.Wrappers;

namespace AppyNox.Services.Base.API.ExceptionExtensions
{
    /// <summary>
    /// Represents a wrapper object for API validation exceptions, extending the standard error response with validation errors.
    /// </summary>
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