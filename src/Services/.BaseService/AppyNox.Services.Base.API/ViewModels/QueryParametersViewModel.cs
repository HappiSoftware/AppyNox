using AppyNox.Services.Base.Domain.Common;
using AppyNox.Services.Base.Infrastructure.Repositories.Common;

namespace AppyNox.Services.Base.API.ViewModels
{
    /// <summary>
    /// Represents a view model for query parameters, extending the base query parameters with additional functionality.
    /// </summary>
    public class QueryParametersViewModel : QueryParametersBase
    {
        #region [ Hidden Properties ]

        /// <summary>
        /// Gets the common DTO level for the query, hidden from public access. Hides it from Swagger.
        /// </summary>
        protected new CommonDtoLevelEnums CommonDtoLevel
        {
            get => base.CommonDtoLevel;
        }

        /// <summary>
        /// Gets or sets the access type for the query, allowing only protected access. Hides it from Swagger.
        /// </summary>
        protected new DtoLevelMappingTypes AccessType
        {
            get => base.AccessType;
            set => base.AccessType = value;
        }

        #endregion
    }
}