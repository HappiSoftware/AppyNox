using AppyNox.Services.Base.Application.Dtos;
using AppyNox.Services.Coupon.Application.Dtos.CouponDetailDtos.Models;
using AppyNox.Services.Coupon.Application.Dtos.CouponDtos.Models.ValueObjects;

namespace AppyNox.Services.Coupon.Application.Dtos.CouponDtos.Models;

public class CouponDto : CouponCreateDto, IAuditDto
{
    #region [ IAuditDto ]

    public AuditInformation AuditInformation { get; set; } = default!;

    #endregion

    #region [ Relations ]

    public CouponIdDto Id { get; set; } = default!;

    public CouponDetailDto CouponDetail { get; set; } = null!;

    #endregion
}