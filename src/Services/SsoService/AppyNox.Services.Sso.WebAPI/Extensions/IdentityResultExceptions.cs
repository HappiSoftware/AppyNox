using AppyNox.Services.Sso.Domain.Entities;
using AppyNox.Services.Base.Application.ExceptionExtensions;
using FluentValidation.Results;
using Microsoft.AspNetCore.Identity;

namespace AppyNox.Services.Sso.WebAPI.Extensions
{
    public static class IdentityResultExceptions
    {
        #region Public Methods

        public static void HandleValidationResult(this IdentityResult identityResult)
        {
            if (!identityResult.Succeeded)
            {
                ValidationResult validationResult = new();
                foreach (var error in identityResult.Errors)
                {
                    ValidationFailure validationFailure = new(error.Code, error.Description);
                    validationResult.Errors.Add(validationFailure);
                }
                throw new FluentValidationException(typeof(ApplicationRole), validationResult);
            }
        }

        #endregion
    }
}