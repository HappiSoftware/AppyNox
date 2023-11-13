using AutoMapper;
using System.Security.Claims;

namespace AppyNox.Services.Authentication.Application.Dtos.ClaimDtos
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