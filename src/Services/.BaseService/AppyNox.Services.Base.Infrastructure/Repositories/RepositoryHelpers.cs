using AppyNox.Services.Base.Application.Dtos;
using AppyNox.Services.Base.Domain.Interfaces;
using AppyNox.Services.Base.Infrastructure.ExceptionExtensions.Base;
using AppyNox.Services.Base.Infrastructure.Repositories.Common;
using Microsoft.EntityFrameworkCore;
using System.Collections;
using System.Linq.Expressions;
using System.Net;

namespace AppyNox.Services.Base.Infrastructure.Repositories;

internal static class RepositoryHelpers
{
    #region [ Projection ]

    private static bool IsSimpleType(Type type)
    {
        if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>))
        {
            // If it's a nullable type, check if the underlying type is simple.
            return IsSimpleType(Nullable.GetUnderlyingType(type)!);
        }

        if (type == typeof(Guid) || type == typeof(Guid?))
        {
            return true;
        }

        var typeCode = Type.GetTypeCode(type);
        switch (typeCode)
        {
            case TypeCode.Empty:
            case TypeCode.Object:
                return false;

            default:
                return true;
        }
    }

    private static bool IsValueObject(Type type)
    {
        return typeof(IValueObject).IsAssignableFrom(type);
    }

    internal static Expression<Func<TEntity, object>> CreateProjection<TEntity>(Type dtoType) where TEntity : class
    {
        var parameterExpr = Expression.Parameter(typeof(TEntity), "entity");
        bool checkNestedNavigation = false;
        HashSet<Type> visitedTypes = [];
        if (!dtoType.Name.Contains("Dto", StringComparison.OrdinalIgnoreCase))
        {
            checkNestedNavigation = true;
            visitedTypes = [dtoType];
        }
        var bindings = CreateBindings(parameterExpr, typeof(TEntity), dtoType, visitedTypes, checkNestedNavigation);
        var body = Expression.MemberInit(Expression.New(dtoType), bindings);
        return Expression.Lambda<Func<TEntity, object>>(body, parameterExpr);
    }

    private static List<MemberBinding> CreateBindings(Expression source, Type sourceType, Type targetType, HashSet<Type> visitedTypes, bool checkNestedNavigation)
    {
        var bindings = new List<MemberBinding>();

        foreach (var targetProp in targetType.GetProperties())
        {
            if (targetProp.PropertyType == typeof(AuditInformation))
            {
                var auditInfoBindings = MapAuditInfoWithShadowProperties(source);
                var auditInfoInit = Expression.MemberInit(Expression.New(typeof(AuditInformation)), auditInfoBindings);
                bindings.Add(Expression.Bind(targetProp, auditInfoInit));
                continue;
            }

            var sourceProp = sourceType.GetProperty(targetProp.Name);
            if (sourceProp != null)
            {
                Expression propertyExpr = Expression.Property(source, sourceProp);
                MemberBinding binding;

                if (IsSimpleType(targetProp.PropertyType))
                {
                    binding = Expression.Bind(targetProp, propertyExpr);
                }
                else if (typeof(IEnumerable).IsAssignableFrom(targetProp.PropertyType)
                     && targetProp.PropertyType.IsGenericType)
                {
                    // Handle collections
                    var collectionType = targetProp.PropertyType.GetGenericArguments()[0];
                    var entityCollectionType = sourceProp.PropertyType.GetGenericArguments()[0];

                    var selectExpression = CreateCollectionSelectExpression(entityCollectionType, collectionType, visitedTypes, checkNestedNavigation);
                    var callExpression = Expression.Call(
                        typeof(Enumerable), "Select", [entityCollectionType, collectionType],
                        propertyExpr, selectExpression);

                    var toListMethod = typeof(Enumerable).GetMethod("ToList")
                        ?? throw new NoxInfrastructureException("Unable to find the 'ToList' method on the 'Enumerable' class.", (int)NoxInfrastructureExceptionCode.ProjectionError);
                    toListMethod = toListMethod.MakeGenericMethod([collectionType]);
                    var toListExpression = Expression.Call(null, toListMethod, callExpression);

                    binding = Expression.Bind(targetProp, toListExpression);
                }
                else // Complex type
                {
                    if (checkNestedNavigation && !IsValueObject(targetProp.PropertyType))
                    {
                        if (visitedTypes.Contains(targetProp.PropertyType))
                        {
                            continue;
                        }
                        visitedTypes.Add(targetProp.PropertyType);
                    }

                    var nestedBindings = CreateBindings(propertyExpr, sourceProp.PropertyType, targetProp.PropertyType, visitedTypes, checkNestedNavigation);
                    var nestedBody = Expression.MemberInit(Expression.New(targetProp.PropertyType), nestedBindings);
                    binding = Expression.Bind(targetProp, nestedBody);
                }

                bindings.Add(binding);
            }
        }

        return bindings;
    }

    private static LambdaExpression CreateCollectionSelectExpression(Type sourceType, Type targetType, HashSet<Type> visitedTypes, bool checkNestedNavigation)
    {
        var parameter = Expression.Parameter(sourceType, "x");
        var bindings = CreateBindings(parameter, sourceType, targetType, visitedTypes, checkNestedNavigation);
        var body = Expression.MemberInit(Expression.New(targetType), bindings);
        return Expression.Lambda(body, parameter);
    }

    private static List<MemberBinding> MapAuditInfoWithShadowProperties(Expression source)
    {
        var auditPropertyNames = new[] { "CreatedBy", "CreationDate", "UpdatedBy", "UpdateDate" };
        var auditInfoType = typeof(AuditInformation);
        var bindings = new List<MemberBinding>();

        foreach (var propName in auditPropertyNames)
        {
            var targetProp = auditInfoType.GetProperty(propName);
            if (targetProp != null)
            {
                var propertyAccess = Expression.Call(
                    typeof(EF), nameof(EF.Property),
                    [targetProp.PropertyType],
                    source, Expression.Constant(propName));

                var binding = Expression.Bind(targetProp, propertyAccess);
                bindings.Add(binding);
            }
        }

        return bindings;
    }

    #endregion

    #region [ Protection ]

    private static List<string> GetValidSortFields(Type entityType)
    {
        var properties = entityType.GetProperties();
        var validFields = new List<string>();

        foreach (var property in properties)
        {
            validFields.Add(property.Name);

            if (!property.PropertyType.IsPrimitive && property.PropertyType != typeof(string))
            {
                var nestedProperties = property.PropertyType.GetProperties();
                foreach (var nestedProperty in nestedProperties)
                {
                    validFields.Add($"{property.Name}.{nestedProperty.Name}");
                }
            }
        }

        return validFields;
    }

    internal static bool ValidateSortBy(string sortBy, Type entityType)
    {
        if (!IsValidSortBy(sortBy, entityType))
        {
            throw new NoxInfrastructureException("Invalid SortBy parameter.", (int)NoxInfrastructureExceptionCode.SqlInjectionError, (int)HttpStatusCode.BadRequest);
        }
        return true;
    }

    internal static bool IsValidSortBy(string sortBy, Type entityType)
    {
        var validFields = GetValidSortFields(entityType);

        if (string.IsNullOrWhiteSpace(sortBy)) return false;

        // Split by comma to handle multiple sorting fields
        var sortSegments = sortBy.Split(',', StringSplitOptions.RemoveEmptyEntries);

        foreach (var segment in sortSegments)
        {
            var parts = segment.Trim().Split(' ', StringSplitOptions.RemoveEmptyEntries);

            // Check if the segment has either one or two parts: ["FieldName"] or ["FieldName", "asc|desc"]
            if (parts.Length == 0 || parts.Length > 2) return false; // Invalid if there are no parts or more than 2

            var fieldName = parts[0];

            // Validate field name
            if (!validFields.Contains(fieldName, StringComparer.OrdinalIgnoreCase))
                return false;

            // If there's a sorting direction, validate it
            if (parts.Length == 2 && !IsSortingDirectionValid(parts[1]))
                return false;
        }

        return true;
    }

    private static bool IsSortingDirectionValid(string direction)
    {
        return direction.Equals("asc", StringComparison.OrdinalIgnoreCase) || direction.Equals("desc", StringComparison.OrdinalIgnoreCase);
    }

    #endregion
}