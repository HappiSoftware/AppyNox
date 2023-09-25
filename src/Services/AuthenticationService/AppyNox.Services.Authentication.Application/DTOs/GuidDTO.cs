using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppyNox.Services.Authentication.Application.DTOs
{
    public class GuidDTO : IHasGuid
    {
        public string Id { get; set; } = string.Empty;
    }
}
