﻿using AppyNox.Services.Coupon.Domain.Common;
using Microsoft.AspNetCore.Mvc.Filters;

namespace AppyNox.Services.Coupon.WebAPI.Filters
{
    public class QueryParametersActionFilter : IActionFilter
    {
        #region [ Public Methods ]

        public void OnActionExecuting(ActionExecutingContext context)
        {
            var queryParameters = new QueryParameters
            {
                PageNumber = Convert.ToInt32(context.HttpContext.Request.Query["PageNumber"]),
                PageSize = Convert.ToInt32(context.HttpContext.Request.Query["PageSize"]),
                SearchTerm = context.HttpContext.Request.Query["SearchTerm"].ToString(),
                SearchColumns = context.HttpContext.Request.Query["SearchColumns"].ToString(),
                SortBy = context.HttpContext.Request.Query["SortBy"].ToString(),
                SortOrder = context.HttpContext.Request.Query["SortOrder"].ToString(),
                DetailLevel = context.HttpContext.Request.Query["DetailLevel"].ToString()
            };

            context.HttpContext.Items["QueryParameters"] = queryParameters;
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
        }

        #endregion
    }
}