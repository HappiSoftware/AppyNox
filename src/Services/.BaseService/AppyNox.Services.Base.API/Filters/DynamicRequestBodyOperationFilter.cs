using AppyNox.Services.Base.API.Attributes;
using AppyNox.Services.Base.API.Exceptions.Base;
using AppyNox.Services.Base.API.Wrappers;
using AppyNox.Services.Base.Application.DtoUtilities;
using AppyNox.Services.Base.Core.Common;
using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Collections;
using System.Collections.Concurrent;

namespace AppyNox.Services.Base.API.Filters;

public class DynamicRequestBodyOperationFilter(IDtoMappingRegistryBase mappingService) : IOperationFilter
{
    private readonly IDtoMappingRegistryBase _mappingService = mappingService;

    // Caches
    private static readonly ConcurrentDictionary<string, object> _paginatedExampleCache = new ConcurrentDictionary<string, object>();
    private static readonly ConcurrentDictionary<string, object> _exampleCache = new ConcurrentDictionary<string, object>();

    public void Apply(OpenApiOperation operation, OperationFilterContext context)
    {
        if(context.ApiDescription.HttpMethod == null)
        {
            return;
        }

        if (context.ApiDescription.HttpMethod.Equals("GET", StringComparison.OrdinalIgnoreCase))
        {
            HandleGetOperation(operation, context);
        }
        else if (context.ApiDescription.HttpMethod.Equals("POST", StringComparison.OrdinalIgnoreCase))
        {
            HandlePostOperation(operation, context);
        }
    }

    private void HandlePostOperation(OpenApiOperation operation, OperationFilterContext context)
    {
        var attribute = context.MethodInfo
            .GetCustomAttributes(true)
            .OfType<SwaggerDynamicRequestBodyAttribute>()
            .FirstOrDefault();

        if (attribute != null)
        {
            var possibleTypes = _mappingService.GetDtoTypesForEntity(attribute.EntityType, attribute.MappingType);
            if (possibleTypes != null && possibleTypes.Count != 0)
            {
                var oneOfSchema = new OpenApiSchema
                {
                    OneOf = possibleTypes.Select(type => context.SchemaGenerator.GenerateSchema(type.Value, context.SchemaRepository)).ToList()
                };

                var mediaType = new OpenApiMediaType
                {
                    Schema = oneOfSchema,
                    Examples = new Dictionary<string, OpenApiExample>()
                };

                foreach (var type in possibleTypes)
                {
                    var exampleInstance = CreateExampleInstance(type.Value);
                    var displayName = type.Key;
                    mediaType.Examples.Add(displayName, new OpenApiExample
                    {
                        Summary = $"Example for {displayName}",
                        Value = new OpenApiString(JsonConvert.SerializeObject(exampleInstance))
                    });
                }

                operation.RequestBody = new OpenApiRequestBody
                {
                    Content = new Dictionary<string, OpenApiMediaType>
                    {
                        ["application/json"] = mediaType
                    }
                };
            }
        }
    }

    private void HandleGetOperation(OpenApiOperation operation, OperationFilterContext context)
    {
        var attribute = context.MethodInfo
            .GetCustomAttributes(true)
            .OfType<SwaggerDynamicRequestBodyAttribute>()
            .FirstOrDefault();

        if (attribute != null)
        {
            var possibleTypes = _mappingService.GetDtoTypesForEntity(attribute.EntityType, attribute.MappingType);
            if (possibleTypes != null && possibleTypes.Any())
            {
                OpenApiSchema schemaToUse;
                if (attribute.IsPaginatedListResponse)
                {
                    // If response should be paginated, wrap possible types in PaginatedList schema
                    schemaToUse = GeneratePaginatedListSchema(possibleTypes.Values, context);
                }
                else
                {
                    // If response is not paginated, directly use the possible types
                    var dataOneOfSchema = new OpenApiSchema
                    {
                        OneOf = possibleTypes.Select(type => context.SchemaGenerator.GenerateSchema(type.Value, context.SchemaRepository)).ToList()
                    };
                    schemaToUse = GenerateNoxApiResponseSchema(dataOneOfSchema);
                }

                if (!operation.Responses.ContainsKey("200"))
                    operation.Responses.Add("200", new OpenApiResponse { Description = "OK" });

                var mediaType = new OpenApiMediaType
                {
                    Schema = schemaToUse,
                    Examples = new Dictionary<string, OpenApiExample>()
                };

                GenerateExamplesForSchema(mediaType, possibleTypes, context, attribute.IsPaginatedListResponse);

                operation.Responses["200"].Content.Clear();
                operation.Responses["200"].Content.Add("application/json", mediaType);
            }
        }
    }

    #region [ Get Operation Private Methods ]

    private static void GenerateExamplesForSchema(OpenApiMediaType mediaType, Dictionary<string, Type> possibleTypes, OperationFilterContext context, bool isPaginated)
    {
        foreach (var type in possibleTypes)
        {
            var exampleInstance = isPaginated
                ? CreatePaginatedExampleInstance(type.Value)
                : new NoxApiResponse(CreateExampleInstance(type.Value)!);
            var displayName = type.Key;
            mediaType.Examples.Add(displayName, new OpenApiExample
            {
                Summary = $"Example for {displayName}",
                Value = new OpenApiString(JsonConvert.SerializeObject(exampleInstance))
            });
        }
    }

    private static OpenApiSchema GenerateNoxApiResponseSchema(OpenApiSchema dataOneOfSchema)
    {
        return new OpenApiSchema
        {
            Type = "object",
            Properties = new Dictionary<string, OpenApiSchema>
            {
                ["message"] = new OpenApiSchema { Type = "string" },
                ["version"] = new OpenApiSchema { Type = "string" },
                ["hasError"] = new OpenApiSchema { Type = "boolean" },
                ["code"] = new OpenApiSchema { Type = "integer", Format = "int32" },
                ["result"] = new OpenApiSchema
                {
                    Type = "object",
                    Properties = new Dictionary<string, OpenApiSchema>
                    {
                        ["data"] = dataOneOfSchema,
                        ["error"] = new OpenApiSchema { Type = "string" }
                    }
                }
            },
            Required = new HashSet<string>
                {
                    "message",
                    "version",
                    "hasError",
                    "code",
                    "result"
                }
        };
    }

    private static OpenApiSchema GeneratePaginatedListSchema(IEnumerable<Type> possibleTypes, OperationFilterContext context)
    {
        var itemOneOfSchema = new OpenApiSchema
        {
            OneOf = possibleTypes.Select(type => context.SchemaGenerator.GenerateSchema(type, context.SchemaRepository)).ToList()
        };
        
        var paginatedListSchema = new OpenApiSchema
        {
            Type = "object",
            Properties = new Dictionary<string, OpenApiSchema>
            {
                ["itemsCount"] = new OpenApiSchema { Type = "integer", Format = "int32" },
                ["totalCount"] = new OpenApiSchema { Type = "integer", Format = "int32" },
                ["currentPage"] = new OpenApiSchema { Type = "integer", Format = "int32" },
                ["pageSize"] = new OpenApiSchema { Type = "integer", Format = "int32" },
                ["items"] = new OpenApiSchema
                {
                    Type = "array",
                    Items = itemOneOfSchema
                }
            },
            Required = new HashSet<string> { "itemsCount", "totalCount", "currentPage", "pageSize", "items" }
        };

        return GenerateNoxApiResponseSchema(paginatedListSchema);
    }

    private static NoxApiResponse CreatePaginatedExampleInstance(Type itemType)
    {
        string typeName = itemType.FullName
            ?? throw new NoxApiException("Type name cannot be null", (int)NoxApiExceptionCode.SwaggerGenerationException);

        if (_paginatedExampleCache.TryGetValue(typeName, out object? instance))
        {
            return new NoxApiResponse(instance, "Get Request Successful.");
        }

        var exampleItem = CreateExampleInstance(itemType);
        var listType = typeof(PaginatedList<>).MakeGenericType([itemType]);
        var paginatedListInstance = Activator.CreateInstance(listType);

        var itemsList = Activator.CreateInstance(typeof(List<>).MakeGenericType(itemType));

        var addMethod = itemsList!.GetType().GetMethod("Add");
        addMethod?.Invoke(itemsList, [exampleItem!]);
        paginatedListInstance!.GetType().GetProperty("Items")?.SetValue(paginatedListInstance, itemsList);

        // Manually set to 0 to prevent negative values
        var itemsCountProperty = listType.GetProperty("ItemsCount");
        itemsCountProperty?.SetValue(paginatedListInstance, 1);

        var totalCountProperty = listType.GetProperty("TotalCount");
        totalCountProperty?.SetValue(paginatedListInstance, 1);

        var currentPageProperty = listType.GetProperty("CurrentPage");
        currentPageProperty?.SetValue(paginatedListInstance, 1);

        var pageSizeProperty = listType.GetProperty("PageSize");
        pageSizeProperty?.SetValue(paginatedListInstance, 1);

        _paginatedExampleCache.TryAdd(typeName, paginatedListInstance);

        return new NoxApiResponse(paginatedListInstance, "Get Request Successful.");
    }

    #endregion

    private static object CreateExampleInstance(Type type)
    {
        string typeName = type.FullName
            ?? throw new NoxApiException("Type name cannot be null", (int)NoxApiExceptionCode.SwaggerGenerationException);

        if (_exampleCache.TryGetValue(typeName, out object? instance))
        {
            return instance;
        }

        instance = Activator.CreateInstance(type);

        foreach (var property in type.GetProperties())
        {
            if (property.PropertyType == typeof(string))
            {
                property.SetValue(instance, "Sample Text", null);
            }
            else if (property.PropertyType == typeof(int) || property.PropertyType == typeof(int?))
            {
                property.SetValue(instance, 1, null);
            }
            else if (property.PropertyType == typeof(Guid) || property.PropertyType == typeof(Guid?))
            {
                property.SetValue(instance, Guid.NewGuid(), null);
            }
            else if (property.PropertyType == typeof(bool))
            {
                property.SetValue(instance, true, null);
            }
            else if (property.PropertyType == typeof(DateTime) || property.PropertyType == typeof(DateTime?))
            {
                property.SetValue(instance, DateTime.Now, null);
            }
            else if (property.PropertyType != typeof(string) && typeof(IEnumerable).IsAssignableFrom(property.PropertyType))
            {
                Type elementType = property.PropertyType.GetGenericArguments()[0];
                IList listInstance = (IList)Activator.CreateInstance(typeof(List<>).MakeGenericType(elementType))!;
                var elementInstance = CreateExampleInstance(elementType);
                if (elementInstance != null)
                {
                    listInstance.Add(elementInstance);
                }
                property.SetValue(instance, listInstance);
            }
            else if (!property.PropertyType.IsPrimitive && !property.PropertyType.IsEnum && property.PropertyType != typeof(string))
            {
                var complexInstance = CreateExampleInstance(property.PropertyType);
                if (complexInstance != null)
                {
                    property.SetValue(instance, complexInstance);
                }
            }
        }
        _exampleCache.TryAdd(typeName, instance 
            ?? throw new NoxApiException($"Could not instantiate example instance for {type.Name}", (int)NoxApiExceptionCode.SwaggerGenerationException));
        return instance;
    }
}