using FluentValidation;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AppyNox.Services.Authentication.Application.Account;

namespace AppyNox.Services.Authentication.Application.Validators
{
    public class LoginDTOValidator : AbstractValidator<LoginDTO>
    {
        public LoginDTOValidator()
        {
            RuleFor(user => user.UserName).NotNull().NotEmpty().NotEmpty().WithMessage("Username is required.").WithErrorCode("422");
            RuleFor(login => login.Password).NotNull().NotEmpty().NotEmpty().WithMessage("Password is required.").WithErrorCode("422");
        }
    }
}