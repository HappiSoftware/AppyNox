namespace AppyNox.Services.Base.Core.Common;

public class PaginatedList<TEntity>
{
    #region [ Properties ]

    public int ItemsCount { get; set; }

    public int TotalCount { get; set; }

    public int CurrentPage { get; set; }

    public int PageSize { get; set; }

    public int TotalPages => (int)Math.Ceiling(TotalCount / (double)PageSize);

    public IEnumerable<TEntity> Items { get; set; } = [];

    #endregion
}