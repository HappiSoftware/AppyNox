using AppyNox.Services.Base.Application.Exceptions;
using AppyNox.Services.Base.Application.Interfaces.Loggers;
using AppyNox.Services.Base.Application.Interfaces.Repositories;
using AppyNox.Services.Base.Core.AsyncLocals;
using AppyNox.Services.Base.Core.Exceptions.Base;
using AppyNox.Services.Coupon.Application.Dtos.CouponDtos.Models.Base;
using AppyNox.Services.Coupon.Application.Dtos.CouponDtos.Models.Extended;
using AppyNox.Services.Coupon.Application.Exceptions.Base;
using AppyNox.Services.Coupon.Application.MediatR.Commands;
using AppyNox.Services.Coupon.Domain.Coupons;
using AutoMapper;
using FluentValidation;
using MediatR;
using System.Net;
using CouponRoot = AppyNox.Services.Coupon.Domain.Coupons.Coupon;

namespace AppyNox.Services.Coupon.Application.MediatR.Handlers;

public class UpdateCouponCommandHandler(
        INoxRepository<CouponRoot> repository,
        IMapper mapper,
        IValidator<CouponExtendedUpdateDto> validator,
        INoxApplicationLogger logger,
        IUnitOfWork unitOfWork)

    : IRequestHandler<UpdateCouponCommand>
{
    #region [ Fields ]

    private readonly INoxRepository<CouponRoot> _repository = repository;

    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    private readonly IMapper _mapper = mapper;

    private readonly INoxApplicationLogger _logger = logger;

    private readonly IValidator<CouponExtendedUpdateDto> _validator = validator;

    #endregion

    #region [ Public Methods ]

    public async Task Handle(UpdateCouponCommand request, CancellationToken cancellationToken)
    {
        try
        {
            _logger.LogInformation($"Updating entity ID: '{request.Dto.Id}' type '{typeof(CouponRoot).Name}'");
            if (request.Id != request.Dto.Id.Value)
            {
                throw new NoxCouponApplicationException("Ids do not match.", (int)NoxCouponApplicationExceptionCode.IdsMismatch, (int)HttpStatusCode.BadRequest);
            }

            #region [ FluentValidation ]

            var validationResult = _validator.Validate(request.Dto);
            if (!validationResult.IsValid)
            {
                throw new NoxFluentValidationException(typeof(CouponExtendedUpdateDto), validationResult);
            }

            #endregion

            CouponId couponId = _mapper.Map<CouponIdDto, CouponId>(request.Dto.Id);
            CouponRoot entity = (await _repository.GetByIdAsync(couponId)
                ?? throw new NoxCouponApplicationException("GetByIdAsync returned null", (int)NoxCouponApplicationExceptionCode.UnexpectedUpdateCommandError));

            _repository.Update(entity);

            entity.UpdateMinimumAmount(request.Dto.Amount.MinAmount);

            entity.UpdateDetail(request.Dto.Detail);

            await _unitOfWork.SaveChangesAsync();
        }
        catch (Exception ex) when (ex is INoxException)
        {
            throw;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error updating entity of type '{typeof(CouponRoot).Name}' with ID: {request.Id}.");
            throw new NoxCouponApplicationException(exceptionCode: (int)NoxCouponApplicationExceptionCode.UnexpectedUpdateCommandError, innerException: ex);
        }
    }

    #endregion
}