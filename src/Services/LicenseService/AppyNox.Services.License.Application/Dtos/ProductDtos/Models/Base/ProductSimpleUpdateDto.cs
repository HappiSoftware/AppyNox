using AppyNox.Services.Base.Application.Dtos;
using AppyNox.Services.License.Application.Dtos.ProductDtos.DetailLevel;

namespace AppyNox.Services.License.Application.Dtos.ProductDtos.Models.Base
{
    [ProductDetailLevel(ProductUpdateDetailLevel.Simple)]
    public class ProductSimpleUpdateDto : ProductSimpleCreateDto, IUpdateDto
    {
        #region [ Properties ]

        public Guid Id { get; set; }

        #endregion
    }
}