using AppyNox.Services.Base.Domain.Common;
using AppyNox.Services.Base.Infrastructure.Repositories.Common;
using AppyNox.Services.Coupon.Application.Dtos.DtoUtilities;
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
using AppyNox.Services.Coupon.Application.UnitTest.DtoTests.Fixtures;

namespace AppyNox.Services.Coupon.Application.UnitTest.DtoTests.DtoUtilitiesTests.CouponDtos
{
    [Collection("DtoMappingRegistry Collection")]
    public class CouponDtoMappingRegistryUnitTests(DtoMappingRegistryFixture fixture)
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
            // Act
            var result = _registry.GetDtoType(DtoLevelMappingTypes.DataAccess, typeof(CouponEntity), CouponDataAccessDetailLevel.Simple.GetDisplayName());

            // Assert
            Assert.NotNull(result);
            Assert.Equal(typeof(CouponSimpleDto), result);
        }

        [Fact]
        public void CouponDataAccessExtendedShouldReturnCouponWithAllRelationsDto()
        {
            // Act
            var result = _registry.GetDtoType(DtoLevelMappingTypes.DataAccess, typeof(CouponEntity), CouponDataAccessDetailLevel.WithAllRelations.GetDisplayName());

            // Assert
            Assert.NotNull(result);
            Assert.Equal(typeof(CouponWithAllRelationsDto), result);
        }

        #endregion

        #region [ Create ]

        [Fact]
        public void CouponCreateSimpleShouldReturnCouponSimpleCreateDto()
        {
            // Act
            var result = _registry.GetDtoType(DtoLevelMappingTypes.Create, typeof(CouponEntity), CouponCreateDetailLevel.Simple.GetDisplayName());

            // Assert
            Assert.NotNull(result);
            Assert.Equal(typeof(CouponSimpleCreateDto), result);
        }

        [Fact]
        public void CouponCreateExtendedShouldReturnCouponExtendedCreateDto()
        {
            // Act
            var result = _registry.GetDtoType(DtoLevelMappingTypes.Create, typeof(CouponEntity), CouponCreateDetailLevel.Extended.GetDisplayName());

            // Assert
            Assert.NotNull(result);
            Assert.Equal(typeof(CouponExtendedCreateDto), result);
        }

        #endregion

        #region [ Update ]

        [Fact]
        public void CouponUpdateSimpleShouldReturnCouponSimpleUpdateDto()
        {
            // Act
            var result = _registry.GetDtoType(DtoLevelMappingTypes.Update, typeof(CouponEntity), CouponUpdateDetailLevel.Simple.GetDisplayName());

            // Assert
            Assert.NotNull(result);
            Assert.Equal(typeof(CouponSimpleUpdateDto), result);
        }

        #endregion

        #endregion
    }
}