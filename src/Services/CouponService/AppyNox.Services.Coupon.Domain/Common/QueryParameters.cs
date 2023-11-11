using AppyNox.Services.Coupon.Domain.Interfaces.Common;
using System.ComponentModel.DataAnnotations;

namespace AppyNox.Services.Coupon.Domain.Common
{
    public class QueryParameters : IQueryParameters
    {
        #region [ Properties ]

        [Range(1, int.MaxValue, ErrorMessage = "Page number must be greater than 0.")]
        public int PageNumber { get; set; } = 1;

        [Range(1, 100, ErrorMessage = "Page size must be between 1 and 100.")]
        public int PageSize { get; set; } = 10;

        // Search
        public string? SearchTerm { get; set; }

        public string? SearchColumns { get; set; } // comma separated

        // Sorting
        public string? SortBy { get; set; }

        public string SortOrder { get; set; } = "asc";

        public string? DetailLevel { get; set; }

        #endregion
    }
}