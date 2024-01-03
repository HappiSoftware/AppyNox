using AppyNox.Services.License.Application.Dtos.LicenseDtos.Models.Base;
using AppyNox.Services.License.Domain.Entities;
using AutoMapper;

namespace AppyNox.Services.License.Application.Dtos.LicenseDtos.Mappings
{
    public class LicenseMapping : Profile
    {
        #region [ Public Constructors ]

        public LicenseMapping()
        {
            CreateMap<LicenseSimpleCreateDto, LicenseEntity>();

            CreateMap<LicenseSimpleUpdateDto, LicenseEntity>().ReverseMap();

            CreateMap<LicenseEntity, LicenseSimpleDto>().ReverseMap();
        }

        #endregion
    }
}