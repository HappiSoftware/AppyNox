using AppyNox.Services.Base.Application.Dtos;
using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Text.Json;

namespace AppyNox.Services.Base.API.Helpers
{
    /// <summary>
    /// Provides helper methods for handling validation in the API layer.
    /// </summary>
    public static class ValidationHelpers
    {
        #region [ Public Methods ]

        /// <summary>
        /// Processes FluentValidation results and updates the model state in an action context.
        /// </summary>
        /// <param name="modelState">The model state dictionary to be updated.</param>
        /// <param name="validationResult">The FluentValidation result.</param>
        /// <param name="actionContext">The action context in which the model state is used.</param>
        public static void HandleValidationResult(ModelStateDictionary modelState, ValidationResult validationResult, ActionContext actionContext)
        {
            foreach (var error in validationResult.Errors)
            {
                modelState.AddModelError(error.ErrorCode, error.ErrorMessage);
            }

            // Use the actionContext to set the ModelState
            actionContext.ModelState.Merge(modelState);
        }

        /// <summary>
        /// Extracts the 'Id' value from a dynamic DTO object.
        /// </summary>
        /// <param name="dto">The dynamic DTO object.</param>
        /// <returns>The extracted 'Id' value as a Guid.</returns>
        public static Guid GetIdFromDynamicDto(dynamic dto)
        {
            var dtoObject = JsonSerializer.Deserialize<UpdateDtoBase>(dto, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            return (Guid)dtoObject.Id;
        }

        #endregion
    }
}