using AppyNox.Services.Base.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppyNox.Services.Coupon.Domain.Common
{
    public class QueryParameters : QueryParametersBase
    {
        #region [ Public Constructors ]

        public QueryParameters() : base()
        {
        }

        public QueryParameters(CommonDtoLevelEnums commonDtoLevel) : base(commonDtoLevel)
        {
        }

        public new static QueryParametersBase CreateForIdOnly()
        {
            return new QueryParameters(CommonDtoLevelEnums.IdOnly) as QueryParametersBase;
        }

        #endregion
    }
}