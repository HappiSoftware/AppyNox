using AppyNox.Services.Base.Application.DtoUtilities;
using AppyNox.Services.Base.Application.Helpers;
using AppyNox.Services.Base.Domain.Common;
using AppyNox.Services.Base.Infrastructure.Repositories.Common;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Dynamic;

namespace AppyNox.Services.Authentication.WebAPI.Controllers.Base
{
    public class BaseController : ControllerBase
    {
        private readonly IDtoMappingRegistryBase _dtoMappingRegistry;
        private readonly IMapper _mapper;
        public BaseController(IDtoMappingRegistryBase dtoMappingRegistry, IMapper mapper)
        {
            _dtoMappingRegistry = dtoMappingRegistry;
            _mapper = mapper;
        }
        protected Type? CreateProjection<TEntity>(QueryParametersBase queryParameters) where TEntity : class
        {
            Type? dtoType = null;

            switch (queryParameters.CommonDtoLevel)
            {
                case CommonDtoLevelEnums.None:
                    dtoType = _dtoMappingRegistry.GetDtoType(queryParameters.AccessType, typeof(TEntity), queryParameters.DetailLevel);
                    break;

                case CommonDtoLevelEnums.Simple:
                    dtoType = _dtoMappingRegistry.GetDtoType(queryParameters.AccessType, typeof(TEntity), CommonDtoLevelEnums.Simple.GetDisplayName());
                    break;

                case CommonDtoLevelEnums.IdOnly:
                    dtoType = typeof(ExpandoObject);
                    break;
            }
            
            return dtoType;
        }

        protected List<object> GetMappedList<T>(List<T> entities, QueryParametersBase queryParameters)
        {
            var dtoType = CreateProjection<IdentityRole>(queryParameters);
            List<object> resultList = [];

            foreach (var entity in entities)
            {
                if (dtoType == typeof(ExpandoObject) && queryParameters.CommonDtoLevel == CommonDtoLevelEnums.IdOnly)
                {
                    // Create dynamic object with only Id property
                    var dynamicObject = new ExpandoObject() as IDictionary<string, object>;
                    dynamicObject["Id"] = entity.GetType().GetProperty("Id")!.GetValue(entity)!;
                    resultList.Add(dynamicObject);
                }
                else
                {
                    var mappedEntity = _mapper.Map(entity, entity.GetType(), dtoType);
                    resultList.Add(mappedEntity);
                }
            }

            return resultList;
        }
    }
}
