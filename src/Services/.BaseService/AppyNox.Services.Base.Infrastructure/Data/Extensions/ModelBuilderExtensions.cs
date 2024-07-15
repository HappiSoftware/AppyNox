using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace AppyNox.Services.Base.Infrastructure.Data.Extensions;

public static class ModelBuilderExtensions
{
    public static void NoxAddGlobalFilter<TEntity>(
        this ModelBuilder modelBuilder,
        Expression<Func<TEntity, bool>> newFilter) where TEntity : class
    {
        var entityType = modelBuilder.Entity<TEntity>();

        var existingFilter = entityType.Metadata.GetQueryFilter();
        if (existingFilter != null)
        {
            var combinedFilter = CombineExpressions<TEntity>(
                (Expression<Func<TEntity, bool>>)existingFilter, newFilter);
            entityType.HasQueryFilter(combinedFilter);
        }
        else
        {
            entityType.HasQueryFilter(newFilter);
        }
    }

    private static Expression<Func<T, bool>> CombineExpressions<T>(
        Expression<Func<T, bool>> expr1, Expression<Func<T, bool>> expr2)
    {
        var parameter = Expression.Parameter(typeof(T));

        var body = Expression.AndAlso(
            Expression.Invoke(expr1, parameter),
            Expression.Invoke(expr2, parameter));

        return Expression.Lambda<Func<T, bool>>(body, parameter);
    }
}