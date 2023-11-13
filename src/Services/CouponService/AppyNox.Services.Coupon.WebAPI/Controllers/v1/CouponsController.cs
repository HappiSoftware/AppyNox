using AppyNox.Services.Coupon.Application.Dtos.Coupon.Models;
using AppyNox.Services.Coupon.Application.ExceptionExtensions;
using AppyNox.Services.Coupon.Application.Services.Implementations;
using AppyNox.Services.Coupon.Domain.Common;
using AppyNox.Services.Coupon.Domain.Entities;
using AppyNox.Services.Coupon.WebAPI.Helpers;
using Asp.Versioning;
using AutoWrapper.Wrappers;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;

namespace AppyNox.Services.Coupon.WebAPI.Controllers.v1
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/[controller]")]
    public class CouponsController : ControllerBase
    {
        #region [ Fields ]

        private readonly GenericService<CouponEntity, CouponWithIdDto, CouponCreateDto, CouponUpdateDto> _couponService;

        private readonly IValidator<CouponCreateDto> _couponValidator;

        #endregion

        #region [ Public Constructors ]

        public CouponsController(
            GenericService<CouponEntity, CouponWithIdDto, CouponCreateDto, CouponUpdateDto> couponService,
            IValidator<CouponCreateDto> couponValidator)
        {
            _couponService = couponService;
            _couponValidator = couponValidator;
        }

        #endregion

        #region [ Public Methods ]

        [HttpGet]
        public async Task<ApiResponse> GetAll([FromQuery] QueryParameters queryParameters)
        {
            try
            {
                var families = await _couponService.GetAllAsync(queryParameters, queryParameters.DetailLevel);
                return new ApiResponse(families, 200);
            }
            catch (DetailLevelNotFoundException exception)
            {
                throw new ApiProblemDetailsException(exception.Message, statusCode: 400);
            }
        }

        [HttpGet("{id}")]
        public async Task<ApiResponse> GetById(Guid id, [FromQuery] QueryParameters queryParameters)
        {
            var coupon = await _couponService.GetByIdAsync(id, queryParameters.DetailLevel);
            if (coupon == null)
            {
                throw new ApiProblemDetailsException($"Record with id: {id} does not exist.", 404);
            }
            return new ApiResponse(coupon);
        }

        [HttpPost]
        public async Task<IActionResult> Create(CouponCreateDto couponDto)
        {
            var validationResult = _couponValidator.Validate(couponDto);
            ValidationHandler.HandleValidationResult(ModelState, validationResult);

            var (guid, basicDto) = await _couponService.AddAsync(couponDto);
            return CreatedAtAction(nameof(GetById), new { id = guid }, basicDto);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, CouponUpdateDto couponDto)
        {
            var validationResult = _couponValidator.Validate(couponDto);
            ValidationHandler.HandleValidationResult(ModelState, validationResult);

            var existingCoupon = await _couponService.GetByIdAsync(id);
            if (existingCoupon == null)
            {
                return NotFound(new ApiResponse(404, $"Coupon with id {id} not found"));
            }

            await _couponService.UpdateAsync(couponDto);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var coupon = await _couponService.GetByIdAsync(id, "WithId");
            if (coupon == null)
            {
                return NotFound(new ApiResponse(404, $"Coupon with id {id} not found"));
            }

            await _couponService.DeleteAsync(coupon);
            return NoContent();
        }

        #endregion
    }
}