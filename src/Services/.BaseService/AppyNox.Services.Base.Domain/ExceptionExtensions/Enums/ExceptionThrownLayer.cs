using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppyNox.Services.Base.Domain.ExceptionExtensions.Enums
{
    public enum ExceptionThrownLayer
    {
        [Display(Name = "Nox Domain Base")]
        DomainBase,

        [Display(Name = "Nox Application Base")]
        ApplicationBase,

        [Display(Name = "Nox Infrastructure Base")]
        InfrastructureBase,

        [Display(Name = "Nox Api Base")]
        ApiBase
    }
}