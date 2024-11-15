using AppyNox.Services.Base.Application.Validators;
using AppyNox.Services.Coupon.Application.Dtos.TicketDtos.Models;
using AppyNox.Services.Coupon.Application.Validators.Ticket.Create;
using FluentValidation;

namespace AppyNox.Services.Coupon.Application.Validators.Ticket.Update;

public class TicketSimpleUpdateValidator : DtoValidatorBase<TicketUpdateDto>
{
    #region [ Public Constructors ]

    public TicketSimpleUpdateValidator(TicketSimpleCreateValidator validator)
    {
        RuleFor(ticket => ticket.Title)
            .NotNull().NotEmpty().WithMessage("Title can not be null");

        RuleFor(ticket => ticket.Content)
            .NotNull().NotEmpty().WithMessage("Content can not be null");
    }

    #endregion
}