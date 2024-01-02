using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppyNox.Services.Authentication.Domain.Entities
{
    public class ApplicationUser : IdentityUser
    {
        #region Properties

        public int CompanyId { get; set; }

        public CompanyEntity Company { get; set; }

        #endregion
    }
}