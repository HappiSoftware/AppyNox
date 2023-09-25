using FluentValidation;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AppyNox.Services.Authentication.Application.DTOs.IdentityUserDTOs.Models;
using AppyNox.Services.Authentication.Application.Validators.SharedRules;
using AppyNox.Services.Authentication.Infrastructure.Data;

namespace AppyNox.Services.Authentication.Application.Validators
{
    public class IdentityUserCreateDTOValidator : AbstractValidator<IdentityUserCreateDTO>
    {
        public IdentityUserCreateDTOValidator(IdentityDbContext context)
        {
            RuleFor(user => user.UserName).CheckUserNameValidity(context);
            RuleFor(x => x.Password).NotNull().NotEmpty().WithMessage("Password required").WithErrorCode("PASSWORD_REQUIRED");
            RuleFor(x => x.Password).Equal(x => x.ConfirmPassword).WithMessage("Passwords should match").WithErrorCode("PASSWORDS_SHOULD_MATCH");
            RuleFor(x => x.Email).CheckEmailValidity(context);
        }

    }


}
