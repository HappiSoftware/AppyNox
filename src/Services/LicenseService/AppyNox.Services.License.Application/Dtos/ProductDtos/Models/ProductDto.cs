using AppyNox.Services.License.Application.Dtos.ProductDtos.Models.Base;

namespace AppyNox.Services.License.Application.Dtos.ProductDtos.Models
{
    public class ProductDto : ProductCreateDto
    {
        #region [ Properties ]

        public ProductIdDto Id { get; set; } = default!;

        #endregion
    }
}