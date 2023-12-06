using AppyNox.Services.Base.Domain.Common;

namespace AppyNox.Services.Base.Application.DtoUtilities
{
    public interface IDtoMappingRegistryBase
    {
        Type GetDetailLevelType(DtoLevelMappingTypes type, Type entityType);
        Dictionary<DtoLevelMappingTypes, Type> GetDetailLevelTypes(Type entityType);
        Type GetDtoType(DtoLevelMappingTypes detailLevelEnum, Type entityType, string detailLevelDescription);
    }
}