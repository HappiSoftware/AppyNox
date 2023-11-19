using System.ComponentModel.DataAnnotations;
using AppyNox.Services.Coupon.Domain.Interfaces.Common;

namespace AppyNox.Services.Coupon.Domain.Common
{
    public class QueryParameters : IQueryParameters
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
                _accessType = value switch
                {
                    "DataAccess" => DtoLevelMappingTypes.DataAccess,
                    "Update" => DtoLevelMappingTypes.Update,
                    "Create" => DtoLevelMappingTypes.Create,
                    _ => DtoLevelMappingTypes.DataAccess
                };
            }
        }

        public string DetailLevel
        {
            get => _detailLevel;
            set
            {
                if (string.IsNullOrEmpty(value))
                {
                    _detailLevel = "Simple";
                    _commonDtoLevel = CommonDtoLevelEnums.Simple;
                }
                else
                {
                    _detailLevel = value;
                    _commonDtoLevel = value switch
                    {
                        "Simple" => CommonDtoLevelEnums.Simple,
                        _ => CommonDtoLevelEnums.None
                    };
                }
            }
        }

        #endregion

        #region [ Public Constructors ]

        public QueryParameters()
        {
            DetailLevel = string.Empty;
        }

        public static QueryParameters CreateForIdOnly()
        {
            return new QueryParameters(CommonDtoLevelEnums.IdOnly);
        }

        #endregion

        #region [ Private Constructors ]

        private QueryParameters(CommonDtoLevelEnums commonDtoLevel) : this()
        {
            switch (commonDtoLevel)
            {
                case CommonDtoLevelEnums.Simple:
                    DetailLevel = "Simple";
                    _commonDtoLevel = CommonDtoLevelEnums.Simple;
                    break;

                case CommonDtoLevelEnums.IdOnly:
                    DetailLevel = "IdOnly";
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