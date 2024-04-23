using AppyNox.Services.Base.Core.Enums;

namespace AppyNox.Services.Base.API.Attributes;

[AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
public class SwaggerDynamicRequestBodyAttribute(Type entityType, DtoLevelMappingTypes mappingType, bool isPaginatedListResponse = false) : Attribute
{
    public Type EntityType { get; } = entityType;
    public DtoLevelMappingTypes MappingType { get; } = mappingType;

    public bool IsPaginatedListResponse { get; } = isPaginatedListResponse;
}