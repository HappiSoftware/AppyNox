using AppyNox.Services.License.Application.Dtos.ProductDtos.Models;
using AppyNox.Services.License.Application.Dtos.ProductDtos.Models.Base;
using AppyNox.Services.License.Domain.Entities;
using AutoMapper;

namespace AppyNox.Services.License.Application.Dtos.ProductDtos.Mappings
{
    public class ProductProfile : Profile
    {
        #region [ Public Constructors ]

        public ProductProfile()
        {
            CreateMap<ProductCreateDto, ProductEntity>().ReverseMap();

            CreateMap<ProductUpdateDto, ProductEntity>().ReverseMap();

            CreateMap<ProductEntity, ProductDto>();

            CreateMap<ProductId, ProductIdDto>().ReverseMap();
        }

        #endregion
    }
}