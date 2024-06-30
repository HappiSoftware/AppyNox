using AppyNox.Services.Base.Application.Constants;
using AppyNox.Services.Base.Application.Extensions;
using AppyNox.Services.Base.Application.Interfaces.Repositories;
using AppyNox.Services.Base.Core.Enums;
using System.ComponentModel.DataAnnotations;

namespace AppyNox.Services.Base.Infrastructure.Repositories.Common;

/// <summary>
/// Provides a base implementation for query parameters with common functionality.
/// </summary>
public abstract class QueryParametersBase : IQueryParameters
{
    #region [ Properties ]

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

    private string _detailLevel = CommonDetailLevels.Simple;

    public string DetailLevel
    {
        get => _detailLevel;

        set
        {
            if (value.Equals("Simple", StringComparison.OrdinalIgnoreCase) || value.IsNullOrEmpty())
            {
                _detailLevel = CommonDetailLevels.Simple;
            }
            else
            {
                _detailLevel = value;
            }
        }
    }

    public string SortBy { get; set; } = string.Empty;

    public string Filter { get; set; } = string.Empty;
    public bool IncludeDeleted { get; set; }

    #endregion
}