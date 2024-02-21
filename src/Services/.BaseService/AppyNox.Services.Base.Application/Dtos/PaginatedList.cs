namespace AppyNox.Services.Base.Application.Dtos;

public class PaginatedList
{
    #region [ Properties ]

    public int ItemsCount { get; set; }

    public int TotalCount { get; set; }

    public int CurrentPage { get; set; }

    public int PageSize { get; set; }

    public int TotalPages => (int)Math.Ceiling(TotalCount / (double)PageSize);

    public IEnumerable<object> Items { get; set; } = [];

    #endregion
}

public class TypedPaginatedList<T> : PaginatedList
{
    #region [ Properties ]

    private IEnumerable<T> _items = [];

    public new IEnumerable<T> Items
    {
        get => _items;

        set => _items = value;
    }

    #endregion
}