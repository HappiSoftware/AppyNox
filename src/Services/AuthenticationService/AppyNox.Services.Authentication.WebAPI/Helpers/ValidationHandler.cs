using AutoWrapper.Wrappers;
using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace AppyNox.Services.Authentication.WebAPI.Helpers
{
    public static class ValidationHandler
    {
        public static void HandleValidationResult(ModelStateDictionary modelState, ValidationResult validationResult)
        {
            if (!validationResult.IsValid)
            {
                foreach (var error in validationResult.Errors)
                {
                    modelState.AddModelError(error.ErrorCode ?? "0", error.ErrorMessage);
                }
                throw new ApiProblemDetailsException(modelState);
            }
        }
    }
}
