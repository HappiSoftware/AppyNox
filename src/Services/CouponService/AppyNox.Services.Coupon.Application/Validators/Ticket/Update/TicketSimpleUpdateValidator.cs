using AppyNox.Services.Base.Application.Validators;
using AppyNox.Services.Coupon.Application.Dtos.TicketDtos.Models.Basic;
using AppyNox.Services.Coupon.Application.Validators.Coupon.Create;

namespace AppyNox.Services.Coupon.Application.Validators.Coupon.Update;

public class TicketSimpleUpdateValidator : DtoValidatorBase<TicketSimpleUpdateDto>
{
    #region [ Public Constructors ]

    public TicketSimpleUpdateValidator(TicketSimpleCreateValidator validator)
    {
        RuleFor(o => o)
            .SetValidator(validator);
    }

    #endregion
}