using AppyNox.Services.Base.API.Helpers;
using AppyNox.Services.Base.API.Wrappers;
using AppyNox.Services.Base.Application.ExceptionExtensions;
using AppyNox.Services.Base.Application.Interfaces.Loggers;
using AppyNox.Services.Base.Core.ExceptionExtensions.Base;
using AutoWrapper.Wrappers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Routing;

namespace AppyNox.Services.Base.API.Middleware
{
    /// <summary>
    /// Middleware for handling exceptions that occur in the request pipeline.
    /// </summary>
    [Obsolete("This Middleware is deprecated and will be removed on v1.0.5. Please use NoxResponseWrapperMiddleware instead.")]
    public class ExceptionHandlingMiddleware(RequestDelegate next, INoxApiLogger logger)
    {
        #region [ Fields ]

        private readonly RequestDelegate _next = next ?? throw new ArgumentNullException(nameof(next));

        private readonly INoxApiLogger _logger = logger;

        #endregion

        #region [ Public Methods ]

        /// <summary>
        /// Invokes the middleware to catch and handle exceptions during request processing.
        /// </summary>
        /// <param name="context">The HTTP context for the current request.</param>
        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (FluentValidationException ex)
            {
                string correlationId = CheckCorrelationIdsAndReturn(context, ex);

                ActionContext actionContext = new(context, context.GetRouteData(), new ControllerActionDescriptor());
                ModelStateDictionary modelState = new();
                ValidationHelpers.HandleValidationResult(modelState, ex.ValidationResult, actionContext);
                throw new ApiException(new NoxApiValidationExceptionWrapObject(ex, Guid.Parse(correlationId), ex.ValidationResult.Errors), statusCode: ex.StatusCode);
            }
            catch (NoxException ex)
            {
                string correlationId = CheckCorrelationIdsAndReturn(context, ex);
                throw new ApiException(new NoxApiExceptionWrapObject(ex, Guid.Parse(correlationId)), statusCode: ex.StatusCode);
            }
            catch (Exception ex)
            {
                throw new ApiException(ex, statusCode: 500);
            }
        }

        #endregion

        #region [ Private Methods ]

        /// <summary>
        /// Checks the CorrelationId from the request header and exception property.
        /// <para>NoxExceptionProperty of correlation id is set from CorrelationContext</para>
        /// </summary>
        private string CheckCorrelationIdsAndReturn(HttpContext context, NoxException ex)
        {
            string correlationId = (context.Request.Headers["X-Correlation-ID"].FirstOrDefault() ?? string.Empty).ToString();
            if (correlationId.Equals(ex.CorrelationId.ToString()))
            {
                return correlationId;
            }
            else
            {
                Guid tempCorrelationId = Guid.NewGuid();
                _logger.LogCritical(new Exception(), $"Correlation Ids mismatched!!! Header: '{correlationId}', CorrelationContext: {ex.CorrelationId}", tempCorrelationId);
                return $"Malformed Correlation Id. Please provide '{tempCorrelationId} to customer service.'";
            }
        }

        #endregion
    }
}