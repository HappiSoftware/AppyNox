using AppyNox.Services.License.Application.Dtos.LicenseDtos.Models;
using AppyNox.Services.License.Application.Dtos.LicenseDtos.Models.ValueObjects;
using AppyNox.Services.License.Domain.Entities;
using AutoMapper;

namespace AppyNox.Services.License.Application.Dtos.LicenseDtos.Mappings
{
    public class LicenseProfile : Profile
    {
        #region [ Public Constructors ]

        public LicenseProfile()
        {
            CreateMap<LicenseCreateDto, LicenseEntity>().ReverseMap();

            CreateMap<LicenseUpdateDto, LicenseEntity>().ReverseMap();

            CreateMap<LicenseEntity, LicenseDto>();

            CreateMap<LicenseId, LicenseIdDto>().ReverseMap();
        }

        #endregion
    }
}