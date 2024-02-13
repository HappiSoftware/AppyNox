namespace AppyNox.Services.Base.Application.Dtos;

public class PaginatedList
{
    #region [ Properties ]

    public IEnumerable<object> Items { get; set; } = [];

    public int ItemsCount { get; set; }

    public int TotalCount { get; set; }

    public int CurrentPage { get; set; }

    public int PageSize { get; set; }

    public int TotalPages => (int)Math.Ceiling(TotalCount / (double)PageSize);

    #endregion
}