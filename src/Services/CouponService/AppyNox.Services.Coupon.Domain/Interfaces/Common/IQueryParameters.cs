using AppyNox.Services.Coupon.Domain.Common;

namespace AppyNox.Services.Coupon.Domain.Interfaces.Common
{
    public interface IQueryParameters
    {
        #region Properties

        string Access { get; set; }

        DtoLevelMappingTypes AccessType { get; }

        CommonDtoLevelEnums CommonDtoLevel { get; }

        string DetailLevel { get; set; }

        int PageNumber { get; set; }

        int PageSize { get; set; }

        #endregion
    }
}