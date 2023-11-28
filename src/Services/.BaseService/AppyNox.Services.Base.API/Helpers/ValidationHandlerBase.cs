using AutoWrapper.Extensions;
using AutoWrapper.Wrappers;
using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace AppyNox.Services.Base.API.Helpers
{
    public static class ValidationHandlerBase
    {
        #region [ Public Methods ]

        public static void HandleValidationResult(ModelStateDictionary modelState, ValidationResult validationResult, ActionContext actionContext)
        {
            foreach (var error in validationResult.Errors)
            {
                modelState.AddModelError(error.ErrorCode, error.ErrorMessage);
            }

            // Use the actionContext to set the ModelState
            actionContext.ModelState.Merge(modelState);
        }

        #endregion
    }
}