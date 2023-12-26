using AppyNox.Services.Base.API.ExceptionExtensions;
using AppyNox.Services.Base.API.Helpers;
using AppyNox.Services.Base.Application.ExceptionExtensions;
using AppyNox.Services.Base.Domain.ExceptionExtensions.Base;
using AutoWrapper.Extensions;
using AutoWrapper.Wrappers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Routing;

namespace AppyNox.Services.Base.API.Middleware
{
    public class ExceptionHandlingMiddleware(RequestDelegate next)
    {
        #region Fields

        private readonly RequestDelegate _next = next ?? throw new ArgumentNullException(nameof(next));

        #endregion

        #region Public Methods

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (FluentValidationException ex)
            {
                string correlationId = (context.Items["CorrelationId"] ?? string.Empty).ToString() ?? string.Empty;
                var actionContext = new ActionContext(context, context.GetRouteData(), new ControllerActionDescriptor());
                var modelState = new ModelStateDictionary();
                ValidationHelpers.HandleValidationResult(modelState, ex.ValidationResult, actionContext);
                throw new ApiException(new NoxApiValidationExceptionWrapObject(ex, correlationId, modelState.AllErrors()), statusCode: ex.StatusCode);
            }
            catch (NoxException ex)
            {
                string correlationId = (context.Items["CorrelationId"] ?? string.Empty).ToString() ?? string.Empty;
                throw new ApiException(new NoxApiExceptionWrapObject(ex, correlationId), statusCode: ex.StatusCode);
            }
            catch (Exception ex)
            {
                throw new ApiException(ex, statusCode: 500);
            }
        }

        #endregion
    }
}