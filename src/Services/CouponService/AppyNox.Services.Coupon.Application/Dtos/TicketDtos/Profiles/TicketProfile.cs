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
        CreateMap<Ticket, TicketSimpleDto>();
        CreateMap<Ticket, TicketExtendedDto>();
        CreateMap<TicketSimpleCreateDto, Ticket>();
        CreateMap<TicketSimpleUpdateDto, Ticket>();
    }

    #endregion
}