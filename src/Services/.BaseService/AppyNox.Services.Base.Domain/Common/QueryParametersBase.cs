using AppyNox.Services.Base.Domain.Interfaces.Common;
using System.ComponentModel.DataAnnotations;

namespace AppyNox.Services.Base.Domain.Common
{
    public abstract class QueryParametersBase : IQueryParameters
    {
        #region [ Properties ]

        private CommonDtoLevelEnums _commonDtoLevel = CommonDtoLevelEnums.None;

        private string _detailLevel = string.Empty;

        private DtoLevelMappingTypes _accessType = DtoLevelMappingTypes.DataAccess;

        [Range(1, int.MaxValue, ErrorMessage = "Page number must be greater than 0.")]
        public int PageNumber { get; set; } = 1;

        [Range(1, 100, ErrorMessage = "Page size must be between 1 and 100.")]
        public int PageSize { get; set; } = 10;

        public CommonDtoLevelEnums CommonDtoLevel { get => _commonDtoLevel; }

        public DtoLevelMappingTypes AccessType
        {
            get => _accessType;
            internal set => _accessType = value;
        }

        /// <summary>
        /// Used only for converting AccessType, dont use this variable in code.
        /// </summary>
        public string Access
        {
            get => string.Empty;
            set
            {
                _accessType = Enum.TryParse<DtoLevelMappingTypes>(value, true, out var result)
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

                _commonDtoLevel = (_detailLevel.Equals("Simple", StringComparison.OrdinalIgnoreCase))
                    ? CommonDtoLevelEnums.Simple
                    : CommonDtoLevelEnums.None;
            }
        }

        #endregion

        #region [ Public Static Methods ]

        /// <summary>
        /// This method should be overridden by derived classes.
        /// </summary>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public static QueryParametersBase CreateForIdOnly()
        {
            throw new NotImplementedException("CreateForIdOnly must be implemented in derived classes.");
        }

        #endregion

        #region [ Protected Constructors ]

        protected QueryParametersBase()
        {
            DetailLevel = string.Empty;
        }

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