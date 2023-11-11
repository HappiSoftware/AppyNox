using AppyNox.Services.Authentication.Application.DtoUtilities;

namespace AppyNox.Services.Authentication.WebAPI.Helpers
{
    public class DtoMappingHelper<TEntity> where TEntity : class
    {
        #region [ Fields ]

        private readonly DTOMappingRegistry _dtoMappingRegistry;

        private readonly Type _detailLevelsEnumType;

        #endregion

        #region [ Public Constructors ]

        public DtoMappingHelper(DTOMappingRegistry dtoMappingRegistry)
        {
            _dtoMappingRegistry = dtoMappingRegistry;
            _detailLevelsEnumType = _dtoMappingRegistry.GetDetailLevelType(typeof(TEntity));
        }

        #endregion

        #region [ Public Methods ]

        public Type GetLeveledDtoType(string? detailLevel)
        {
            if (string.IsNullOrEmpty(detailLevel))
            {
                detailLevel = _dtoMappingRegistry.GetBasicDetailLevel();
            }

            return _dtoMappingRegistry.GetDTOType(_detailLevelsEnumType, typeof(TEntity), detailLevel);
        }

        #endregion
    }
}