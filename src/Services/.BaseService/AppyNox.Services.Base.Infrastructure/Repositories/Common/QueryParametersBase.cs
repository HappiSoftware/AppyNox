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

        private CommonDtoLevelEnums _commonDtoLevel = CommonDtoLevelEnums.None;

        private string _detailLevel = string.Empty;

        [Range(1, int.MaxValue, ErrorMessage = "Page number must be greater than 0.")]
        public int PageNumber { get; set; } = 1;

        [Range(1, 100, ErrorMessage = "Page size must be between 1 and 100.")]
        public int PageSize { get; set; } = 10;

        public CommonDtoLevelEnums CommonDtoLevel { get => _commonDtoLevel; }

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
                    _commonDtoLevel = CommonDtoLevelEnums.Simple;
                }
                else if (_detailLevel.Equals("IdOnly", StringComparison.OrdinalIgnoreCase))
                {
                    _detailLevel = "IdOnly";
                    _commonDtoLevel = CommonDtoLevelEnums.IdOnly;
                }
                else
                {
                    _commonDtoLevel = CommonDtoLevelEnums.None;
                }
            }
        }

        #endregion

        #region [ Public Static Methods ]

        /// <summary>
        /// Creates a new instance of <see cref="QueryParametersBase"/> for queries that require only the ID.
        /// This method should be overridden in derived classes.
        /// </summary>
        /// <returns>A new <see cref="QueryParametersBase"/> instance.</returns>
        /// <exception cref="NotImplementedException">Thrown when not implemented in derived classes.</exception>
        public static QueryParametersBase CreateForIdOnly()
        {
            throw new NotImplementedException("CreateForIdOnly must be implemented in derived classes.");
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

        /// <summary>
        /// Initializes a new instance of the <see cref="QueryParametersBase"/> class with a specified common DTO level.
        /// </summary>
        /// <param name="commonDtoLevel">The common DTO level for the query.</param>
        /// <exception cref="ArgumentException">Thrown when an invalid common DTO level is provided.</exception>
        protected QueryParametersBase(CommonDtoLevelEnums commonDtoLevel) : this()
        {
            switch (commonDtoLevel)
            {
                case CommonDtoLevelEnums.Simple:
                    DetailLevel = nameof(CommonDtoLevelEnums.Simple);
                    _commonDtoLevel = CommonDtoLevelEnums.Simple;
                    break;

                case CommonDtoLevelEnums.IdOnly:
                    DetailLevel = nameof(CommonDtoLevelEnums.IdOnly);
                    _commonDtoLevel = CommonDtoLevelEnums.IdOnly;
                    break;

                case CommonDtoLevelEnums.None:
                default:
                    throw new ArgumentException("CommonDtoLevel is not provided or set as none for QueryParameters.");
            }
        }

        #endregion
    }
}