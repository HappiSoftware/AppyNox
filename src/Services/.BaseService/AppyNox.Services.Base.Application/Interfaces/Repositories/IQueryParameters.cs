using AppyNox.Services.Base.Core.Enums;

namespace AppyNox.Services.Base.Application.Interfaces.Repositories
{
    /// <summary>
    /// Defines a set of parameters for querying data in a repository.
    /// </summary>
    public interface IQueryParameters
    {
        #region [ Properties ]

        /// <summary>
        /// Gets or sets the access level as a string. Primarily used for conversion.
        /// </summary>
        string Access { get; set; }

        /// <summary>
        /// Gets the access type for the query, indicating the level of data access required. This field is not set from request
        /// parameters, but is instead converted from Access(string) property.
        /// </summary>
        DtoLevelMappingTypes AccessType { get; }

        /// <summary>
        /// Gets or sets the detail level for the query as a string.
        /// This property then will be converted to related Enum type of the TEntity using DtoMappingRegistry.
        /// </summary>
        string DetailLevel { get; set; }

        /// <summary>
        /// Gets or sets the page number for pagination.
        /// </summary>
        int PageNumber { get; set; }

        /// <summary>
        /// Gets or sets the page size for pagination.
        /// </summary>
        int PageSize { get; set; }

        string SortBy { get; set; }

        string Filter { get; set; }

        #endregion
    }
}