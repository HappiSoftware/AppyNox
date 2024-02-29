using AppyNox.Services.Base.Core.Enums;
using AppyNox.Services.Base.Core.Extensions;
using AppyNox.Services.Coupon.Application.Dtos.CouponDtos.DetailLevel;
using AppyNox.Services.Coupon.Application.Dtos.CouponDtos.Models.Base;
using AppyNox.Services.Coupon.Application.Dtos.CouponDtos.Models.Extended;
using AppyNox.Services.Coupon.Application.DtoUtilities;
using AppyNox.Services.Coupon.Application.UnitTest.DtoTests.Fixtures;
using AppyNox.Services.Coupon.Domain.Coupons;
using CouponAggreagate = AppyNox.Services.Coupon.Domain.Coupons.Coupon;

namespace AppyNox.Services.Coupon.Application.UnitTest.DtoTests;

[Collection("DtoMappingRegistry Collection")]
public class CouponDtoMappingRegistryUnitTest(DtoMappingRegistryFixture fixture)
{
    #region [ Fields ]

    private readonly DtoMappingRegistry _registry = fixture.Registry;

    #endregion

    #region [ Dto Level Mapping Types ]

    [Fact]
    public void CouponDataAccessDtoLevelMappingTypesShouldBeAvailable()
    {
        // Act
        var result = _registry.GetDetailLevelTypes(typeof(CouponAggreagate)).GetValueOrDefault(DtoLevelMappingTypes.DataAccess);

        // Assert
        Assert.NotNull(result);
    }

    [Fact]
    public void CouponUpdateDtoLevelMappingTypesShouldBeAvailable()
    {
        // Act
        var result = _registry.GetDetailLevelTypes(typeof(CouponAggreagate)).GetValueOrDefault(DtoLevelMappingTypes.Update);

        // Assert
        Assert.NotNull(result);
    }

    [Fact]
    public void CouponCreateDtoLevelMappingTypesShouldBeAvailable()
    {
        // Act
        var result = _registry.GetDetailLevelTypes(typeof(CouponAggreagate)).GetValueOrDefault(DtoLevelMappingTypes.Create);

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
        var result = _registry.GetDtoType(DtoLevelMappingTypes.DataAccess, typeof(CouponAggreagate), CouponDataAccessDetailLevel.Simple.GetDisplayName());

        // Assert
        Assert.NotNull(result);
        Assert.Equal(typeof(CouponSimpleDto), result);
    }

    [Fact]
    public void CouponDataAccessExtendedShouldReturnCouponWithAllRelationsDto()
    {
        // Act
        var result = _registry.GetDtoType(DtoLevelMappingTypes.DataAccess, typeof(CouponAggreagate), CouponDataAccessDetailLevel.WithAllRelations.GetDisplayName());

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
        var result = _registry.GetDtoType(DtoLevelMappingTypes.Create, typeof(CouponAggreagate), CouponCreateDetailLevel.Simple.GetDisplayName());

        // Assert
        Assert.NotNull(result);
        Assert.Equal(typeof(CouponSimpleCreateDto), result);
    }

    [Fact]
    public void CouponCreateExtendedShouldReturnCouponExtendedCreateDto()
    {
        // Act
        var result = _registry.GetDtoType(DtoLevelMappingTypes.Create, typeof(CouponAggreagate), CouponCreateDetailLevel.Extended.GetDisplayName());

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
        var result = _registry.GetDtoType(DtoLevelMappingTypes.Update, typeof(CouponAggreagate), CouponUpdateDetailLevel.Simple.GetDisplayName());

        // Assert
        Assert.NotNull(result);
        Assert.Equal(typeof(CouponSimpleUpdateDto), result);
    }

    [Fact]
    public void CouponUpdateExtendedShouldReturnCouponExtendedUpdateDto()
    {
        // Act
        var result = _registry.GetDtoType(DtoLevelMappingTypes.Update, typeof(CouponAggreagate), CouponUpdateDetailLevel.Extended.GetDisplayName());

        // Assert
        Assert.NotNull(result);
        Assert.Equal(typeof(CouponExtendedUpdateDto), result);
    }

    #endregion

    #endregion
}