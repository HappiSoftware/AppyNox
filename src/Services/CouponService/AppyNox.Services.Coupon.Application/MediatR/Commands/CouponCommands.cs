using AppyNox.Services.Base.Domain.Interfaces;
using AppyNox.Services.Coupon.Application.Dtos.CouponDtos.Models.Extended;
using MediatR;
using System.Diagnostics.CodeAnalysis;

namespace AppyNox.Services.Coupon.Application.MediatR.Commands;

[SuppressMessage("Sonar Code Smell", "S2326:Unused type parameters should be removed", Justification = "TEntity is used to specify the type of entity being created")]
public record UpdateCouponCommand(Guid Id, CouponExtendedUpdateDto Dto) : IRequest;