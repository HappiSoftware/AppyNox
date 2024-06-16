using AppyNox.Services.Base.Application.Validators;
using AppyNox.Services.Coupon.Application.Dtos.TicketDtos.Models.Basic;
using FluentValidation;

namespace AppyNox.Services.Coupon.Application.Validators.Ticket.Create;

public class TicketSimpleCreateValidator : DtoValidatorBase<TicketSimpleCreateDto>
{
    #region [ Public Constructors ]

    public TicketSimpleCreateValidator()
    {
        RuleFor(ticket => ticket.Title)
            .NotNull().NotEmpty().WithMessage("Title can not be null");

        RuleFor(ticket => ticket.Content)
            .NotNull().NotEmpty().WithMessage("Content can not be null");

        RuleFor(ticket => ticket.ReportDate)
            .NotNull().NotEmpty().WithMessage("ReportDate can not be null");
    }

    #endregion
}