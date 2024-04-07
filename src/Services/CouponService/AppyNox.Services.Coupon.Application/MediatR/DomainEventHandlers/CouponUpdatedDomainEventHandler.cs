using AppyNox.Services.Base.Application.Interfaces.Loggers;
using AppyNox.Services.Base.Application.Interfaces.Repositories;
using AppyNox.Services.Base.Core.ExceptionExtensions.Base;
using AppyNox.Services.Coupon.Application.ExceptionExtensions.Base;
using AppyNox.Services.Coupon.Domain.Coupons;
using MediatR;

namespace AppyNox.Services.Coupon.Application.MediatR.DomainEventHandlers;

internal class CouponUpdatedDomainEventHandler(
    INoxRepository<CouponHistory> repository,
    INoxApplicationLogger logger,
    IUnitOfWork unitOfWork) : INotificationHandler<CouponUpdatedDomainEvent>
{
    private readonly INoxRepository<CouponHistory> _repository = repository;

    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    private readonly INoxApplicationLogger _logger = logger;

    public async Task Handle(CouponUpdatedDomainEvent domainEvent, CancellationToken cancellationToken)
    {
        try
        {
            CouponHistory statusHistory = new(domainEvent.CouponId, domainEvent.MinimumAmount);
            await _repository.AddAsync(statusHistory);
            await _unitOfWork.SaveChangesAsync("System");
            _logger.LogInformation($"Successfully processed CouponUpdatedDomainEvent for CouponId: {domainEvent.CouponId.Value} with MinimumAmount: {domainEvent.MinimumAmount}");
        }
        catch (Exception ex) when (ex is INoxException)
        {
            throw;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error processing domain event '{typeof(CouponUpdatedDomainEvent).Name}' for CouponId: {domainEvent.CouponId.Value}");
            throw new NoxCouponApplicationException(ex, (int)NoxCouponApplicationExceptionCode.UnexpectedDomainEventHandlerError);
        }
    }
}