using AppyNox.Services.Base.API.ExceptionExtensions;
using AppyNox.Services.Base.Domain.ExceptionExtensions.Base;
using AutoWrapper.Wrappers;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppyNox.Services.Base.API.Middlewares
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
            catch (NoxException ex)
            {
                string correlationId = (context.Items["CorrelationId"] ?? string.Empty).ToString() ?? string.Empty;
                throw new ApiException(new NoxApiExceptionWrapObject(ex, correlationId));
            }
        }

        #endregion
    }
}