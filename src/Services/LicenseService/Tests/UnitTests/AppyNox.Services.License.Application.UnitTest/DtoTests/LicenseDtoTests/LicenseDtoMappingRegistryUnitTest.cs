using AppyNox.Services.Base.Domain.Common;
using AppyNox.Services.License.Application.Dtos.DtoUtilities;
using AppyNox.Services.License.Application.Dtos.LicenseDtos.DetailLevel;
using AppyNox.Services.License.Application.Dtos.LicenseDtos.Models.Base;
using AppyNox.Services.License.Application.UnitTest.DtoTests.Fixtures;
using AppyNox.Services.License.Domain.Entities;
using AppyNox.Services.Base.Application.Extensions;

namespace AppyNox.Services.License.Application.UnitTest.DtoTests.LicenseDtoTests
{
    [Collection("DtoMappingRegistry Collection")]
    public class LicenseDtoMappingRegistryUnitTest(DtoMappingRegistryFixture fixture)
    {
        #region [ Fields ]

        private readonly DtoMappingRegistry _registry = fixture.Registry;

        #endregion

        #region [ Dto Level Mapping Types ]

        [Fact]
        public void LicenseDataAccessDtoLevelMappingTypesShouldBeAvailable()
        {
            // Act
            var result = _registry.GetDetailLevelTypes(typeof(LicenseEntity)).GetValueOrDefault(DtoLevelMappingTypes.DataAccess);

            // Assert
            Assert.NotNull(result);
        }

        [Fact]
        public void LicenseUpdateDtoLevelMappingTypesShouldBeAvailable()
        {
            // Act
            var result = _registry.GetDetailLevelTypes(typeof(LicenseEntity)).GetValueOrDefault(DtoLevelMappingTypes.Update);

            // Assert
            Assert.NotNull(result);
        }

        [Fact]
        public void LicenseCreateDtoLevelMappingTypesShouldBeAvailable()
        {
            // Act
            var result = _registry.GetDetailLevelTypes(typeof(LicenseEntity)).GetValueOrDefault(DtoLevelMappingTypes.Create);

            // Assert
            Assert.NotNull(result);
        }

        #endregion

        #region [ Get Dto Types ]

        #region [ DataAccess ]

        [Fact]
        public void LicenseDataAccessSimpleShouldReturnLicenseSimpleDto()
        {
            // Act
            var result = _registry.GetDtoType(DtoLevelMappingTypes.DataAccess, typeof(LicenseEntity), LicenseDataAccessDetailLevel.Simple.GetDisplayName());

            // Assert
            Assert.NotNull(result);
            Assert.Equal(typeof(LicenseSimpleDto), result);
        }

        #endregion

        #region [ Create ]

        [Fact]
        public void LicenseCreateSimpleShouldReturnLicenseSimpleCreateDto()
        {
            // Act
            var result = _registry.GetDtoType(DtoLevelMappingTypes.Create, typeof(LicenseEntity), LicenseCreateDetailLevel.Simple.GetDisplayName());

            // Assert
            Assert.NotNull(result);
            Assert.Equal(typeof(LicenseSimpleCreateDto), result);
        }

        #endregion

        #region [ Update ]

        [Fact]
        public void LicenseUpdateSimpleShouldReturnLicenseSimpleUpdateDto()
        {
            // Act
            var result = _registry.GetDtoType(DtoLevelMappingTypes.Update, typeof(LicenseEntity), LicenseUpdateDetailLevel.Simple.GetDisplayName());

            // Assert
            Assert.NotNull(result);
            Assert.Equal(typeof(LicenseSimpleUpdateDto), result);
        }

        #endregion

        #endregion
    }
}