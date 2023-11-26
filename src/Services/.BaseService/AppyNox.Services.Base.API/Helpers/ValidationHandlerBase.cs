using AutoWrapper.Wrappers;
using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace AppyNox.Services.Base.API.Helpers
{
    public static class ValidationHandlerBase
    {
        #region [ Public Methods ]

        public static void HandleValidationResult(ModelStateDictionary modelState, ValidationResult validationResult)
        {
            if (!validationResult.IsValid)
            {
                foreach (var error in validationResult.Errors)
                {
                    modelState.AddModelError(error.ErrorCode, error.ErrorMessage);
                }
                throw new ApiProblemDetailsException(modelState);
            }
        }

        #endregion
    }
}