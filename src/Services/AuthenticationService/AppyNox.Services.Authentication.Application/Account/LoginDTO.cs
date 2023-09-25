using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppyNox.Services.Authentication.Application.Account
{
    public class LoginDTO
    {
        public string UserName { get; set; } = null!;
        public string Password { get; set; } = null!;
    }
}
