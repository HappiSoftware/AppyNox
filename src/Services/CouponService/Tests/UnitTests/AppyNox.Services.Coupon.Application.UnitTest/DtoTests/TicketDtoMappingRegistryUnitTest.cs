using AppyNox.Services.Base.Application.Constants;
using AppyNox.Services.Base.Core.Enums;
using AppyNox.Services.Coupon.Application.Dtos.TicketDtos.DetailLevel;
using AppyNox.Services.Coupon.Application.Dtos.TicketDtos.Models.Basic;
using AppyNox.Services.Coupon.Application.Dtos.TicketDtos.Models.Extended;
using AppyNox.Services.Coupon.Application.DtoUtilities;
using AppyNox.Services.Coupon.Application.UnitTest.DtoTests.Fixtures;
using AppyNox.Services.Coupon.Domain.Entities;

namespace AppyNox.Services.Coupon.Application.UnitTest.DtoTests;

[Collection("DtoMappingRegistry Collection")]
public class TicketDtoMappingRegistryUnitTest(DtoMappingRegistryFixture fixture)
{
    #region [ Fields ]

    private readonly DtoMappingRegistry _registry = fixture.Registry;

    #endregion

    #region [ Dto Level Mapping Types ]

    [Fact]
    public void TicketDataAccessDtoLevelMappingTypesShouldBeAvailable()
    {
        // Act
        var result = _registry.GetDetailLevelTypes(typeof(Ticket)).GetValueOrDefault(DtoLevelMappingTypes.DataAccess);

        // Assert
        Assert.NotNull(result);
    }

    [Fact]
    public void TicketUpdateDtoLevelMappingTypesShouldBeAvailable()
    {
        // Act
        var result = _registry.GetDetailLevelTypes(typeof(Ticket)).GetValueOrDefault(DtoLevelMappingTypes.Update);

        // Assert
        Assert.NotNull(result);
    }

    [Fact]
    public void TicketCreateDtoLevelMappingTypesShouldBeAvailable()
    {
        // Act
        var result = _registry.GetDetailLevelTypes(typeof(Ticket)).GetValueOrDefault(DtoLevelMappingTypes.Create);

        // Assert
        Assert.NotNull(result);
    }

    #endregion

    #region [ Get Dto Types ]

    #region [ DataAccess ]

    [Fact]
    public void TicketDataAccessSimpleShouldReturnTicketSimpleDto()
    {
        // Act
        var result = _registry.GetDtoType(DtoLevelMappingTypes.DataAccess, typeof(Ticket), CommonDetailLevels.Simple);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(typeof(TicketSimpleDto), result);
    }

    [Fact]
    public void TicketDataAccessExtendedShouldReturnTicketExtendedDto()
    {
        // Act
        var result = _registry.GetDtoType(DtoLevelMappingTypes.DataAccess, typeof(Ticket), CommonDetailLevels.Extended);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(typeof(TicketExtendedDto), result);
    }

    #endregion

    #region [ Create ]

    [Fact]
    public void TicketCreateSimpleShouldReturnCouponSimpleCreateDto()
    {
        // Act
        var result = _registry.GetDtoType(DtoLevelMappingTypes.Create, typeof(Ticket), CommonDetailLevels.Simple);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(typeof(TicketSimpleCreateDto), result);
    }

    #endregion

    #region [ Update ]

    [Fact]
    public void CouponUpdateSimpleShouldReturnCouponSimpleUpdateDto()
    {
        // Act
        var result = _registry.GetDtoType(DtoLevelMappingTypes.Update, typeof(Ticket), CommonDetailLevels.Simple);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(typeof(TicketSimpleUpdateDto), result);
    }

    #endregion

    #endregion
}