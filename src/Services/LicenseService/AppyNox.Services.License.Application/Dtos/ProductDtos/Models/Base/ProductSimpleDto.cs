﻿using AppyNox.Services.License.Application.Dtos.ProductDtos.DetailLevel;

namespace AppyNox.Services.License.Application.Dtos.ProductDtos.Models.Base
{
    [ProductDetailLevel(ProductDataAccessDetailLevel.Simple)]
    public class ProductSimpleDto : ProductSimpleCreateDto
    {
        #region [ Properties ]

        public ProductIdDto Id { get; set; } = default!;

        #endregion
    }
}