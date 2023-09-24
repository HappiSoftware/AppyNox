using CouponService.Domain.Common;
using Microsoft.AspNetCore.Mvc.Filters;

namespace CouponService.WebAPI.Filters
{
    [Obsolete("Deprecated!")]
    public class ValidateQueryParametersAttribute : ActionFilterAttribute
    {
        private readonly Type _dtoType;

        public ValidateQueryParametersAttribute(Type dtoType)
        {
            _dtoType = dtoType;
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var queryParameters = context.ActionArguments["queryParameters"] as QueryParameters;

            if (queryParameters == null)
            {
                return;
            }

            var validColumns = _dtoType.GetProperties().Select(p => p.Name).ToList();

            // Validate search columns
            var columnsToSearch = queryParameters.SearchColumns?.Split(',', StringSplitOptions.RemoveEmptyEntries).ToList();

            if (columnsToSearch == null)
            {
                return;
            }

            foreach (var column in columnsToSearch)
            {
                if (!validColumns.Contains(column))
                {
                    context.ModelState.AddModelError("SearchColumns", $"Invalid search column: {column}");
                }
            }
        }
    }
}
