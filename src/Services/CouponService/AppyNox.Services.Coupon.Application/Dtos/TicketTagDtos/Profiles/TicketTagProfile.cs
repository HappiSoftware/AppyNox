using AppyNox.Services.Coupon.Application.Dtos.TicketTagDtos.Models;
using AppyNox.Services.Coupon.Domain.Entities;
using AutoMapper;

namespace AppyNox.Services.Coupon.Application.Dtos.TicketTagDtos.Profiles;

public class TicketTagProfile : Profile
{
    #region [ Public Constructors ]

    public TicketTagProfile()
    {
        CreateMap<TicketTag, TicketTagDto>();
        CreateMap<TicketTagCreateDto, TicketTag>();
        CreateMap<TicketTagUpdateDto, TicketTag>();
    }

    #endregion
}