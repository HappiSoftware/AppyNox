using AutoMapper;
using System.Security.Claims;

namespace AppyNox.Services.Authentication.Application.DTOs.ClaimDTOs
{
    public class ClaimMapping : Profile
    {
        #region [ Public Constructors ]

        public ClaimMapping()
        {
            CreateMap<ClaimDTO, Claim>().ReverseMap();
        }

        #endregion
    }
}