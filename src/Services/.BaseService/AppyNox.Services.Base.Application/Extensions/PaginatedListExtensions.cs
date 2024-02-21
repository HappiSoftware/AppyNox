using AppyNox.Services.Base.Application.Dtos;

namespace AppyNox.Services.Base.Application.Extensions;

public static class PaginatedListExtensions
{
    #region [ Public Methods ]

    public static TypedPaginatedList<T> ConvertToTypedPaginatedList<T>(this PaginatedList paginatedList)
    {
        var typedPaginatedList = new TypedPaginatedList<T>
        {
            ItemsCount = paginatedList.ItemsCount,
            TotalCount = paginatedList.TotalCount,
            CurrentPage = paginatedList.CurrentPage,
            PageSize = paginatedList.PageSize,
            Items = paginatedList.Items.Cast<T>()
        };

        return typedPaginatedList;
    }

    #endregion
}