using AppyNox.Services.Base.Core.Enums;

namespace AppyNox.Services.Base.Application.Interfaces.Repositories;

/// <summary>
/// Defines a set of parameters for querying data in a repository.
/// </summary>
public interface IQueryParameters
{
    #region [ Properties ]

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

    bool IncludeDeleted { get; set; }

    #endregion
}