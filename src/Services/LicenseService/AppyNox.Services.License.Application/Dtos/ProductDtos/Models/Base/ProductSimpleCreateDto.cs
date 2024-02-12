using AppyNox.Services.Base.Domain.Interfaces;
using AppyNox.Services.License.Application.Dtos.ProductDtos.DetailLevel;

namespace AppyNox.Services.License.Application.Dtos.ProductDtos.Models.Base;

[ProductDetailLevel(ProductCreateDetailLevel.Simple)]
public class ProductSimpleCreateDto : IHasCode
{
    #region [ Properties ]

    public string? Name { get; set; }

    #endregion

    #region [ IHasCode ]

    public string Code { get; set; } = string.Empty;

    #endregion
}