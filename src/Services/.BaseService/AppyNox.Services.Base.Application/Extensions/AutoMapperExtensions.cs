using AppyNox.Services.Base.Application.Dtos;
using AppyNox.Services.Base.Domain.Interfaces;
using AutoMapper;

namespace AppyNox.Services.Base.Application.Extensions
{
    public static class AutoMapperExtensions
    {
        #region [ Public Methods ]

        public static IMappingExpression<TSource, TDestination> MapAuditInfo<TSource, TDestination>(
            this IMappingExpression<TSource, TDestination> mappingExpression)
            where TDestination : class, IAuditDto
            where TSource : class, IAuditableData
        {
            return mappingExpression.ForMember(dest => dest.AuditInfo, opt => opt.MapFrom(src => new AuditInfo
            {
                CreatedBy = src.CreatedBy,
                CreationDate = src.CreationDate,
                UpdatedBy = src.UpdatedBy,
                UpdateDate = src.UpdateDate
            }));
        }

        #endregion
    }
}