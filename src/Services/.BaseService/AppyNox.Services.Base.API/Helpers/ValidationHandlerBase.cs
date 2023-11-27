using AutoWrapper.Extensions;
using AutoWrapper.Wrappers;
using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace AppyNox.Services.Base.API.Helpers
{
    public static class ValidationHandlerBase
    {
        #region [ Public Methods ]

        public static ApiException HandleValidationResult(ModelStateDictionary modelState, ValidationResult validationResult)
        {
            foreach (var error in validationResult.Errors)
            {
                modelState.AddModelError(error.ErrorCode, error.ErrorMessage);
            }
            return new ApiException(modelState.AllErrors());
        }

        #endregion
    }
}