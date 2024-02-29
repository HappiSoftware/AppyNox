using AppyNox.Services.Coupon.Application.Dtos.TicketTagDtos.Models.Basic;
using AppyNox.Services.Coupon.Domain.Entities;
using AutoMapper;

namespace AppyNox.Services.Coupon.Application.Dtos.TicketTagDtos.Profiles;

public class TicketTagProfile : Profile
{
    #region [ Public Constructors ]

    public TicketTagProfile()
    {
        CreateMap<TicketTag, TicketTagSimpleDto>();
        CreateMap<TicketTagSimpleCreateDto, TicketTag>();
        CreateMap<TicketTagSimpleUpdateDto, TicketTag>();
    }

    #endregion
}