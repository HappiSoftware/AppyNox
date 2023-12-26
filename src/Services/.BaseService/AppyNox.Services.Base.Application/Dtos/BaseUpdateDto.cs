using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppyNox.Services.Base.Application.Dtos
{
    public class BaseUpdateDto : IUpdateDto
    {
        public Guid Id { get; set; }
    }
}
