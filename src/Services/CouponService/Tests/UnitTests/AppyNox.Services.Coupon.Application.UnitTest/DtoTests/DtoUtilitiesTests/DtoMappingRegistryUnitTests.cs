using AppyNox.Services.Base.Domain.Common;
using AppyNox.Services.Base.Infrastructure.Repositories.Common;
using AppyNox.Services.Coupon.Application.Dtos.DtoUtilities;
using AppyNox.Services.Coupon.Application.UnitTest.DtoTests.Bases;
using AppyNox.Services.Coupon.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AppyNox.Services.Base.Application.Helpers;
using AppyNox.Services.Coupon.Application.Dtos.CouponDtos.Models.Base;
using AppyNox.Services.Coupon.Application.Dtos.CouponDtos.Models.Extended;
using AppyNox.Services.Coupon.Application.Dtos.CouponDtos.DetailLevel;

namespace AppyNox.Services.Coupon.Application.UnitTest.DtoTests.DtoUtilitiesTests
{
    [Collection("DtoMappingRegistry Collection")]
    public class DtoMappingRegistryUnitTests(DtoMappingRegistryFixture fixture)
    {
        #region [ Fields ]

        private readonly DtoMappingRegistry _registry = fixture.Registry;

        #endregion

        #region [ Dto Level Mapping Types ]

        [Fact]
        public void CouponDataAccessDtoLevelMappingTypesShouldBeAvailable()
        {
            // Act
            var result = _registry.GetDetailLevelTypes(typeof(CouponEntity)).GetValueOrDefault(DtoLevelMappingTypes.DataAccess);

            // Assert
            Assert.NotNull(result);
        }

        [Fact]
        public void CouponUpdateDtoLevelMappingTypesShouldBeAvailable()
        {
            // Act
            var result = _registry.GetDetailLevelTypes(typeof(CouponEntity)).GetValueOrDefault(DtoLevelMappingTypes.Update);

            // Assert
            Assert.NotNull(result);
        }

        [Fact]
        public void CouponCreateDtoLevelMappingTypesShouldBeAvailable()
        {
            // Act
            var result = _registry.GetDetailLevelTypes(typeof(CouponEntity)).GetValueOrDefault(DtoLevelMappingTypes.Create);

            // Assert
            Assert.NotNull(result);
        }

        #endregion

        #region [ Get Dto Types ]

        #region [ DataAccess ]

        [Fact]
        public void CouponDataAccessSimpleShouldReturnCouponSimpleDto()
        {
            // Arrange
            var detailLevelMap = _registry.GetDetailLevelTypes(typeof(CouponEntity)).GetValueOrDefault(DtoLevelMappingTypes.DataAccess);
            Assert.NotNull(detailLevelMap);

            // Act
            var result = _registry.GetDtoType(detailLevelMap, typeof(CouponEntity), CouponDataAccessDetailLevel.Simple.GetDisplayName());

            // Assert
            Assert.NotNull(result);
            Assert.Equal(typeof(CouponSimpleDto), result);
        }

        [Fact]
        public void CouponDataAccessExtendedShouldReturnCouponWithAllRelationsDto()
        {
            // Arrange
            var detailLevelMap = _registry.GetDetailLevelTypes(typeof(CouponEntity)).GetValueOrDefault(DtoLevelMappingTypes.DataAccess);
            Assert.NotNull(detailLevelMap);

            // Act
            var result = _registry.GetDtoType(detailLevelMap, typeof(CouponEntity), CouponDataAccessDetailLevel.WithAllRelations.GetDisplayName());

            // Assert
            Assert.NotNull(result);
            Assert.Equal(typeof(CouponWithAllRelationsDto), result);
        }

        #endregion

        #region [ Create ]

        [Fact]
        public void CouponCreateSimpleShouldReturnCouponSimpleCreateDto()
        {
            // Arrange
            var detailLevelMap = _registry.GetDetailLevelTypes(typeof(CouponEntity)).GetValueOrDefault(DtoLevelMappingTypes.Create);
            Assert.NotNull(detailLevelMap);

            // Act
            var result = _registry.GetDtoType(detailLevelMap, typeof(CouponEntity), CouponCreateDetailLevel.Simple.GetDisplayName());

            // Assert
            Assert.NotNull(result);
            Assert.Equal(typeof(CouponSimpleCreateDto), result);
        }

        [Fact]
        public void CouponCreateExtendedShouldReturnCouponExtendedCreateDto()
        {
            // Arrange
            var detailLevelMap = _registry.GetDetailLevelTypes(typeof(CouponEntity)).GetValueOrDefault(DtoLevelMappingTypes.Create);
            Assert.NotNull(detailLevelMap);

            // Act
            var result = _registry.GetDtoType(detailLevelMap, typeof(CouponEntity), CouponCreateDetailLevel.Extended.GetDisplayName());

            // Assert
            Assert.NotNull(result);
            Assert.Equal(typeof(CouponExtendedCreateDto), result);
        }

        #endregion

        #region [ Update ]

        [Fact]
        public void CouponUpdateSimpleShouldReturnCouponSimpleUpdateDto()
        {
            // Arrange
            var detailLevelMap = _registry.GetDetailLevelTypes(typeof(CouponEntity)).GetValueOrDefault(DtoLevelMappingTypes.Update);
            Assert.NotNull(detailLevelMap);

            // Act
            var result = _registry.GetDtoType(detailLevelMap, typeof(CouponEntity), CouponUpdateDetailLevel.Simple.GetDisplayName());

            // Assert
            Assert.NotNull(result);
            Assert.Equal(typeof(CouponSimpleUpdateDto), result);
        }

        #endregion

        #endregion
    }
}