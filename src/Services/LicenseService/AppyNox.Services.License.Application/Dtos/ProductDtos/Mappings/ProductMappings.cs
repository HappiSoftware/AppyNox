using AppyNox.Services.License.Application.Dtos.ProductDtos.Models.Base;
using AppyNox.Services.License.Domain.Entities;
using AutoMapper;

namespace AppyNox.Services.License.Application.Dtos.ProductDtos.Mappings
{
    public class ProductMappings : Profile
    {
        #region [ Public Constructors ]

        public ProductMappings()
        {
            CreateMap<ProductSimpleCreateDto, ProductEntity>().ReverseMap();

            CreateMap<ProductSimpleUpdateDto, ProductEntity>().ReverseMap();

            CreateMap<ProductEntity, ProductSimpleDto>();
        }

        #endregion
    }
}