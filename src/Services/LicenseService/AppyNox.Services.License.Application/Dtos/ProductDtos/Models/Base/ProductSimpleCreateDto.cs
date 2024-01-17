using AppyNox.Services.Base.Application.Dtos;
using AppyNox.Services.License.Application.Dtos.ProductDtos.DetailLevel;

namespace AppyNox.Services.License.Application.Dtos.ProductDtos.Models.Base
{
    [ProductDetailLevel(ProductCreateDetailLevel.Simple)]
    public class ProductSimpleCreateDto : DtoBase
    {
        #region [ Properties ]

        public string? Name { get; set; }

        #endregion
    }
}