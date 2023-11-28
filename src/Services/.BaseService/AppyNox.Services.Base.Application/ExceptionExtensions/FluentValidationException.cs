using AppyNox.Services.Base.Application.ExceptionExtensions.Base;
using FluentValidation.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppyNox.Services.Base.Application.ExceptionExtensions
{
    public class FluentValidationException(ValidationResult validationResult) : Exception
    {
        #region [ Properties ]

        public ValidationResult ValidationResult { get; } = validationResult;

        #endregion
    }
}