using AppyNox.Services.Base.Domain.Interfaces;

namespace AppyNox.Services.License.Application.Dtos.ProductDtos.Models;

public class ProductCreateDto : IHasCode
{
    #region [ Properties ]

    public string? Name { get; set; }

    #endregion

    #region [ IHasCode ]

    public string Code { get; set; } = string.Empty;

    #endregion
}