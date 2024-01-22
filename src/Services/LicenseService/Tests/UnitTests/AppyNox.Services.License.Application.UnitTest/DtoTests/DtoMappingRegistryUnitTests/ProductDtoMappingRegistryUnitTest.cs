using AppyNox.Services.Base.Core.Enums;
using AppyNox.Services.Base.Core.Extensions;
using AppyNox.Services.License.Application.Dtos.DtoUtilities;
using AppyNox.Services.License.Application.Dtos.ProductDtos.DetailLevel;
using AppyNox.Services.License.Application.Dtos.ProductDtos.Models.Base;
using AppyNox.Services.License.Application.UnitTest.DtoTests.Fixtures;
using AppyNox.Services.License.Domain.Entities;

namespace AppyNox.Services.License.Application.UnitTest.DtoTests.DtoMappingRegistryUnitTests
{
    [Collection("DtoMappingRegistry Collection")]
    public class ProductDtoMappingRegistryUnitTest(DtoMappingRegistryFixture fixture)
    {
        #region [ Fields ]

        private readonly DtoMappingRegistry _registry = fixture.Registry;

        #endregion

        #region [ Dto Level Mapping Types ]

        [Fact]
        public void ProductDataAccessDtoLevelMappingTypesShouldBeAvailable()
        {
            // Act
            var result = _registry.GetDetailLevelTypes(typeof(ProductEntity)).GetValueOrDefault(DtoLevelMappingTypes.DataAccess);

            // Assert
            Assert.NotNull(result);
        }

        [Fact]
        public void ProductUpdateDtoLevelMappingTypesShouldBeAvailable()
        {
            // Act
            var result = _registry.GetDetailLevelTypes(typeof(ProductEntity)).GetValueOrDefault(DtoLevelMappingTypes.Update);

            // Assert
            Assert.NotNull(result);
        }

        [Fact]
        public void ProductCreateDtoLevelMappingTypesShouldBeAvailable()
        {
            // Act
            var result = _registry.GetDetailLevelTypes(typeof(ProductEntity)).GetValueOrDefault(DtoLevelMappingTypes.Create);

            // Assert
            Assert.NotNull(result);
        }

        #endregion

        #region [ Get Dto Types ]

        #region [ DataAccess ]

        [Fact]
        public void ProductDataAccessSimpleShouldReturnProductSimpleDto()
        {
            // Act
            var result = _registry.GetDtoType(DtoLevelMappingTypes.DataAccess, typeof(ProductEntity), ProductDataAccessDetailLevel.Simple.GetDisplayName());

            // Assert
            Assert.NotNull(result);
            Assert.Equal(typeof(ProductSimpleDto), result);
        }

        #endregion

        #region [ Create ]

        [Fact]
        public void ProductCreateSimpleShouldReturnProductSimpleCreateDto()
        {
            // Act
            var result = _registry.GetDtoType(DtoLevelMappingTypes.Create, typeof(ProductEntity), ProductCreateDetailLevel.Simple.GetDisplayName());

            // Assert
            Assert.NotNull(result);
            Assert.Equal(typeof(ProductSimpleCreateDto), result);
        }

        #endregion

        #region [ Update ]

        [Fact]
        public void ProductUpdateSimpleShouldReturnProductSimpleUpdateDto()
        {
            // Act
            var result = _registry.GetDtoType(DtoLevelMappingTypes.Update, typeof(ProductEntity), ProductUpdateDetailLevel.Simple.GetDisplayName());

            // Assert
            Assert.NotNull(result);
            Assert.Equal(typeof(ProductSimpleUpdateDto), result);
        }

        #endregion

        #endregion
    }
}