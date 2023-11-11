using AppyNox.Services.Coupon.Domain.Common;
using Microsoft.AspNetCore.Mvc.Filters;

namespace AppyNox.Services.Coupon.WebAPI.Filters
{
    [Obsolete("Deprecated!")]
    public class ValidateQueryParametersAttribute : ActionFilterAttribute
    {
        #region [ Fields ]

        private readonly Type _dtoType;

        #endregion

        #region [ Public Constructors ]

        public ValidateQueryParametersAttribute(Type dtoType)
        {
            _dtoType = dtoType;
        }

        #endregion

        #region [ Public Methods ]

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

        #endregion
    }
}