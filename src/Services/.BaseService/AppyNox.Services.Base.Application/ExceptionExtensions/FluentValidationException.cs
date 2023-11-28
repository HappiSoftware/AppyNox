using AppyNox.Services.Base.Application.ExceptionExtensions.Base;
using AppyNox.Services.Base.Domain.Common.HttpStatusCodes;
using FluentValidation.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppyNox.Services.Base.Application.ExceptionExtensions
{
    public class FluentValidationException(Type dtoType, ValidationResult validationResult)
        : NoxApplicationException($"Request responded with one or more validation errors for '{dtoType}'", (int)NoxClientErrorResponseCodes.BadRequest)
    {
        #region [ Properties ]

        public ValidationResult ValidationResult { get; } = validationResult;

        #endregion
    }
}