using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CouponService.Application.ExceptionExtensions
{
    public class DetailLevelNotFoundException : Exception
    {
        public DetailLevelNotFoundException()
        { }

        public DetailLevelNotFoundException(string message)
            : base(message)
        { }

        public DetailLevelNotFoundException(string message, Exception innerException)
            : base(message, innerException)
        { }
    }
}
