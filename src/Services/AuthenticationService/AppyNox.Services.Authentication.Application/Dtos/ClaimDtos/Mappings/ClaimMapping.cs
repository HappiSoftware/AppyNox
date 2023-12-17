using AppyNox.Services.Authentication.Application.Dtos.ClaimDtos.Models.Base;
using AutoMapper;
using System.Security.Claims;

namespace AppyNox.Services.Authentication.Application.Dtos.ClaimDtos.Mappings
{
    public class ClaimMapping : Profile
    {
        #region [ Public Constructors ]

        public ClaimMapping()
        {
            CreateMap<ClaimDto, Claim>().ReverseMap();
        }

        #endregion
    }
}