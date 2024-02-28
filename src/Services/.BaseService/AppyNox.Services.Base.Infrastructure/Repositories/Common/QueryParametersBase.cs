using AppyNox.Services.Base.Application.Interfaces.Repositories;
using AppyNox.Services.Base.Core.Enums;
using System.ComponentModel.DataAnnotations;

namespace AppyNox.Services.Base.Infrastructure.Repositories.Common
{
    /// <summary>
    /// Provides a base implementation for query parameters with common functionality.
    /// </summary>
    public abstract class QueryParametersBase : IQueryParameters
    {
        #region [ Properties ]

        private string _detailLevel = string.Empty;

        [Range(1, int.MaxValue, ErrorMessage = "Page number must be greater than 0.")]
        public int PageNumber { get; set; } = 1;

        [Range(1, 100, ErrorMessage = "Page size must be between 1 and 100.")]
        public int PageSize { get; set; } = 10;

        public DtoLevelMappingTypes AccessType { get; set; } = DtoLevelMappingTypes.DataAccess;

        /// <summary>
        /// Used only for converting AccessType, do not use this variable in code.
        /// </summary>
        public string Access
        {
            get => string.Empty;

            set
            {
                AccessType = Enum.TryParse<DtoLevelMappingTypes>(value, true, out var result)
                    ? result
                    : DtoLevelMappingTypes.DataAccess;
            }
        }

        public string DetailLevel
        {
            get => _detailLevel;

            set
            {
                _detailLevel = string.IsNullOrEmpty(value)
                    ? "Simple"
                    : value;

                if (_detailLevel.Equals("Simple", StringComparison.OrdinalIgnoreCase))
                {
                    _detailLevel = "Simple";
                }
                else if (_detailLevel.Equals("IdOnly", StringComparison.OrdinalIgnoreCase))
                {
                    _detailLevel = "IdOnly";
                }
            }
        }

        #endregion

        #region [ Protected Constructors ]

        /// <summary>
        /// Initializes a new instance of the <see cref="QueryParametersBase"/> class with default settings.
        /// </summary>
        protected QueryParametersBase()
        {
            DetailLevel = string.Empty;
        }

        #endregion
    }
}