using AppyNox.Services.Base.API.ExceptionExtensions;
using AppyNox.Services.Base.API.ExceptionExtensions.Base;
using AppyNox.Services.Base.Domain.Common;
using AppyNox.Services.Base.Domain.Common.HttpStatusCodes;
using AppyNox.Services.Base.Domain.ExceptionExtensions.Base;
using AppyNox.Services.Base.Domain.ExceptionExtensions.Enums;
using AutoWrapper.Wrappers;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppyNox.Services.Base.API.Middlewares
{
    public class QueryParameterValidateMiddleware(RequestDelegate next)
    {
        #region Fields

        private readonly RequestDelegate _next = next ?? throw new ArgumentNullException(nameof(next));

        #endregion

        #region Public Methods

        public async Task InvokeAsync(HttpContext context)
        {
            var accessValue = context.Request.Query["Access"].ToString();

            if (!string.IsNullOrEmpty(accessValue) && !Enum.TryParse<DtoLevelMappingTypes>(accessValue, true, out var result))
            {
                throw new NoxApiException($"'{accessValue}' is not a valid Access modifier.", (int)NoxClientErrorResponseCodes.BadRequest);
            }
            await _next(context);
        }

        #endregion
    }
}