using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CouponService.Domain.Interfaces
{
    public interface IEntityWithGuid
    {
        Guid Id { get; set; }
    }
}
