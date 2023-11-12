using AppyNox.Services.Authentication.Application.DtoUtilities;

namespace AppyNox.Services.Authentication.WebAPI.Helpers
{
    public class DtoMappingHelper<TEntity> where TEntity : class
    {
        #region [ Fields ]

        private readonly DtoMappingRegistry _dtoMappingRegistry;

        private readonly Type _detailLevelsEnumType;

        #endregion

        #region [ Public Constructors ]

        public DtoMappingHelper(DtoMappingRegistry dtoMappingRegistry)
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
                detailLevel = _dtoMappingRegistry.BasicDetailLevel;
            }

            return _dtoMappingRegistry.GetDtoType(_detailLevelsEnumType, typeof(TEntity), detailLevel);
        }

        #endregion
    }
}