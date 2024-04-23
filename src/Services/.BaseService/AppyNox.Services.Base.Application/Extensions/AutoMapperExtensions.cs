using AppyNox.Services.Base.Application.Dtos;
using AppyNox.Services.Base.Domain.Interfaces;
using AutoMapper;

namespace AppyNox.Services.Base.Application.Extensions;

public static class AutoMapperExtensions
{
    public static IMappingExpression<TSource, TDestination> MapAuditInformation<TSource, TDestination>(
        this IMappingExpression<TSource, TDestination> mappingExpression)
        where TSource : IAuditable
        where TDestination : IAuditDto
    {
        return mappingExpression.ForMember(dto => dto.AuditInformation, conf => conf.MapFrom(entity => new AuditInformation
        {
            CreatedBy = entity.CreatedBy,
            CreationDate = entity.CreationDate,
            UpdatedBy = entity.UpdatedBy,
            UpdateDate = entity.UpdateDate
        }));
    }
}