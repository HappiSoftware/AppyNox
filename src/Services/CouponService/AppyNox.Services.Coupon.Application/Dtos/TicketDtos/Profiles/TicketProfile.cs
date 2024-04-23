using AppyNox.Services.Base.Application.Extensions;
using AppyNox.Services.Coupon.Application.Dtos.TicketDtos.Models.Basic;
using AppyNox.Services.Coupon.Application.Dtos.TicketDtos.Models.Extended;
using AppyNox.Services.Coupon.Domain.Entities;
using AutoMapper;

namespace AppyNox.Services.Coupon.Application.Dtos.TicketDtos.Profiles;

public class TicketProfile : Profile
{
    #region [ Public Constructors ]

    public TicketProfile()
    {
        CreateMap<Ticket, TicketSimpleDto>().MapAuditInformation();
        CreateMap<Ticket, TicketExtendedDto>()
            .IncludeBase<Ticket, TicketSimpleDto>();
        CreateMap<TicketSimpleCreateDto, Ticket>();
        CreateMap<TicketSimpleUpdateDto, Ticket>();
    }

    #endregion
}