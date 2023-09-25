using AppyNox.Services.Authentication.Application.DtoUtilities;
using Microsoft.AspNetCore.Identity;

namespace AppyNox.Services.Authentication.WebAPI.Helpers
{
    public class DtoMappingHelper<TEntity> where TEntity : class
    {
        private readonly DTOMappingRegistry _dtoMappingRegistry;
        private readonly Type _detailLevelsEnumType;
        public DtoMappingHelper(DTOMappingRegistry dtoMappingRegistry)
        {
            _dtoMappingRegistry = dtoMappingRegistry;
            _detailLevelsEnumType = _dtoMappingRegistry.GetDetailLevelType(typeof(TEntity));
        }

        public Type GetLeveledDtoType(string? detailLevel)
        {
            if (string.IsNullOrEmpty(detailLevel))
            {
                detailLevel = _dtoMappingRegistry.GetBasicDetailLevel();
            }

            return _dtoMappingRegistry.GetDTOType(_detailLevelsEnumType, typeof(TEntity), detailLevel);
        }
    }
}
