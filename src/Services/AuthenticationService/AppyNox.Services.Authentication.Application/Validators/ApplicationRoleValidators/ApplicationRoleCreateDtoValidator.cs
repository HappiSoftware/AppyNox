using AppyNox.Services.Authentication.Application.DTOs.ApplicationRoleDTOs.Models;
using AppyNox.Services.Base.Application.Validators;
using FluentValidation;

namespace AppyNox.Services.Authentication.Application.Validators.ApplicationRoleValidators
{
    public class ApplicationRoleCreateDtoValidator : DtoValidatorBase<ApplicationRoleCreateDto>
    {
        #region Public Constructors

        public ApplicationRoleCreateDtoValidator()
        {
            RuleFor(ar => ar.Description)
                .MaximumLength(60).WithMessage("Description cannot be longer than 60 characters");
        }

        #endregion
    }
}