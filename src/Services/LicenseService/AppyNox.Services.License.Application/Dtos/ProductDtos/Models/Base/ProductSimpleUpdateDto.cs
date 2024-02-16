using AppyNox.Services.Base.Application.Dtos;
using AppyNox.Services.License.Application.Dtos.ProductDtos.DetailLevel;

namespace AppyNox.Services.License.Application.Dtos.ProductDtos.Models.Base
{
    [ProductDetailLevel(ProductUpdateDetailLevel.Simple)]
    public class ProductSimpleUpdateDto : ProductSimpleCreateDto
    {
        #region [ Properties ]

        public ProductIdDto Id { get; set; } = default!;

        #endregion
    }
}