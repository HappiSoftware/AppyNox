using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppyNox.Services.Authentication.Domain.Entities
{
    public class ApplicationUser : IdentityUser
    {
        #region [ Properties ]

        public Guid CompanyId { get; set; }

        public virtual CompanyEntity Company { get; set; } = null!;

        #endregion
    }
}