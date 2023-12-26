using AppyNox.Services.Base.Application.Dtos;
using AutoWrapper.Extensions;
using AutoWrapper.Wrappers;
using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Text.Json;

namespace AppyNox.Services.Base.API.Helpers
{
    public static class ValidationHelpers
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

        public static Guid GetIdFromDynamicDto(dynamic dto)
        {
            var dtoObject = JsonSerializer.Deserialize<BaseUpdateDto>(dto, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            return (Guid)dtoObject.Id;
        }

        #endregion
    }
}