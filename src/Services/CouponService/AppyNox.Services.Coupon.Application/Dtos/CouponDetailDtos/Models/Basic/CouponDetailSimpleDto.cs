using AppyNox.Services.Base.Domain.Interfaces;
using AppyNox.Services.Coupon.Application.Dtos.CouponDetailDtos.DetailLevel;
using AppyNox.Services.Coupon.Application.Dtos.CouponDetailTagDtos.Models.Basic;
using AppyNox.Services.Coupon.Domain.Entities;

namespace AppyNox.Services.Coupon.Application.Dtos.CouponDetailDtos.Models.Basic
{
    [CouponDetailDetailLevel(CouponDetailDataAccessDetailLevel.Simple)]
    public class CouponDetailSimpleDto : IHasCode
    {
        #region [ Properties ]

        public Guid Id { get; set; }

        // Omitted for simplicity

        #endregion

        #region [ IHasCode ]

        public string Code { get; set; } = string.Empty;

        #endregion

        #region [ Relations ]

        public virtual ICollection<CouponDetailTagSimpleDto>? CouponDetailTags { get; set; }

        #endregion
    }
}