using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppyNox.Services.Base.Domain.Common
{
    public enum DtoLevelMappingTypes
    {
        [Display(Name = "DataAccess")]
        DataAccess,

        [Display(Name = "Create")]
        Create,

        [Display(Name = "Update")]
        Update
    }
}