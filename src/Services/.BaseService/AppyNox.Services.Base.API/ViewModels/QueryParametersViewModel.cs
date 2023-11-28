using AppyNox.Services.Base.Domain.Common;

namespace AppyNox.Services.Base.API.ViewModels
{
    public class QueryParametersViewModel : QueryParametersBase
    {
        #region [ Hidden Properties ]

        protected new CommonDtoLevelEnums CommonDtoLevel
        {
            get => base.CommonDtoLevel;
        }

        protected new DtoLevelMappingTypes AccessType
        {
            get => base.AccessType;
            set => base.AccessType = value;
        }

        #endregion
    }
}