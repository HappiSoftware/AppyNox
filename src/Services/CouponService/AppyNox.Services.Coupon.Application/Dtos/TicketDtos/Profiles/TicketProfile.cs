using AppyNox.Services.Base.Application.Extensions;
using AppyNox.Services.Coupon.Application.Dtos.TicketDtos.Models;
using AppyNox.Services.Coupon.Domain.Entities;
using AutoMapper;

namespace AppyNox.Services.Coupon.Application.Dtos.TicketDtos.Profiles;

public class TicketProfile : Profile
{
    #region [ Public Constructors ]

    public TicketProfile()
    {
        CreateMap<Ticket, TicketDto>().MapAuditInformation();
        CreateMap<TicketCreateDto, Ticket>();
        CreateMap<TicketUpdateDto, Ticket>();
    }

    #endregion
}