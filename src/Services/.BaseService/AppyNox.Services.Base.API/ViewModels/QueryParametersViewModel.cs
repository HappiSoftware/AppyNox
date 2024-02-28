using AppyNox.Services.Base.Core.Enums;
using AppyNox.Services.Base.Infrastructure.Repositories.Common;

namespace AppyNox.Services.Base.API.ViewModels;

/// <summary>
/// Represents a view model for query parameters, extending the base query parameters with additional functionality.
/// <para>Use for v1.0 endpoints</para>
/// </summary>
public class QueryParametersViewModel : QueryParametersBase
{
    #region [ Hidden Properties ]

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

public class QueryParametersViewModelBasic : QueryParametersViewModel
{
    #region [ Hidden Properties ]

    /// <summary>
    /// Gets or sets the access type for the query, allowing only protected access. Hides it from Swagger.
    /// </summary>
    protected new DtoLevelMappingTypes AccessType
    {
        get => base.AccessType;
    }

    protected new string Access
    {
        get => base.Access;
    }

    protected new string DetailLevel
    {
        get => base.DetailLevel;
    }

    #endregion
}