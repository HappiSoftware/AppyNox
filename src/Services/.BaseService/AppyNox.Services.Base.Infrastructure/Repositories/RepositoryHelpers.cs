using AppyNox.Services.Base.Application.Dtos;
using AppyNox.Services.Base.Domain.Interfaces;
using AppyNox.Services.Base.Infrastructure.ExceptionExtensions.Base;
using Microsoft.EntityFrameworkCore;
using System.Collections;
using System.Linq.Expressions;
using System.Net;

namespace AppyNox.Services.Base.Infrastructure.Repositories;

public static class RepositoryHelpers
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

    private static readonly HashSet<string> Blacklist = new(StringComparer.OrdinalIgnoreCase)
    {
        // General SQL manipulation and potentially harmful actions
        "drop", "insert", "delete", "update", "exec", "execute", "merge", "declare", "alter", "create",
        "xp_", "sp_", "--", ";", "/*", "*/", "cast", "convert", "table", "from", "select",

        // Potentially dangerous functions and system views
        "sysobjects", "syscolumns", "@@", "db_name", "bulk", "admin",

        // Commands related to database structure manipulation or data theft
        "grant", "revoke", "deny", "link", "openquery", "opendatasource", "openrowset", "dump", "restore",

        // Keywords that might be used in unwanted data manipulation or exfiltration
        "cursor", "fetch", "kill", "session_user", "system_user", "table_name", "column_name", "schema", "information_schema",

        // PostgreSQL-specific additions
        "pg_", // Prefix for PostgreSQL system catalogs and functions
        "setval", "currval", "nextval", // Functions for manipulating sequences
        "regclass", // Cast to regclass to get an object's OID by name, which could reveal schema information
        "::", // Type cast operator, could be used in payload obfuscation
        "plpythonu", "plperlu", // Untrusted procedural languages
        "dblink", // Used to execute queries across databases
        "pg_sleep", // Could be used in blind SQL injection attacks to measure response times

        // Additional PostgreSQL-specific items to consider
        "lo_import", "lo_export", // Large Object operations that might be misused
        "pg_read_file", "pg_ls_dir", // Functions that can read server files or list directory contents
    };

    public static bool IsValidExpression(string expression)
    {
        // Ensure the expression doesn't contain any blacklisted terms
        if (Blacklist.Any(blacklisted => expression.Contains(blacklisted, StringComparison.OrdinalIgnoreCase)))
        {
            throw new NoxInfrastructureException("Malicious behavior detected.", (int)NoxInfrastructureExceptionCode.SqlInjectionError, (int)HttpStatusCode.BadRequest);
        }
        return true;
    }

    #endregion
}