namespace AppyNox.Services.Coupon.Domain.Interfaces.Common
{
    public interface IQueryParameters
    {
        #region [ Properties ]

        string DetailLevel { get; set; }

        int PageNumber { get; set; }

        int PageSize { get; set; }

        string? SearchColumns { get; set; }

        string? SearchTerm { get; set; }

        string? SortBy { get; set; }

        string SortOrder { get; set; }

        #endregion
    }
}