using AutoWrapper.Wrappers;
using CouponService.Application.DTOs.Coupon.Models;
using CouponService.Application.ExceptionExtensions;
using CouponService.Application.Services.Interfaces;
using CouponService.Domain.Common;
using CouponService.Domain.Entities;
using CouponService.WebAPI.Filters;
using CouponService.WebAPI.Helpers;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;

namespace CouponService.WebAPI.Controllers.v1
{
    [ApiController]
    [Route("api/[controller]")]
    public class CouponsController : ControllerBase
    {
        private readonly IGenericService<CouponEntity, CouponWithIdDTO, CouponCreateDTO, CouponUpdateDTO> _couponService;
        private readonly IValidator<CouponCreateDTO> _couponValidator;

        public CouponsController(
            IGenericService<CouponEntity, CouponWithIdDTO, CouponCreateDTO, CouponUpdateDTO> couponService,
            IValidator<CouponCreateDTO> couponValidator)
        {
            _couponService = couponService;
            _couponValidator = couponValidator;
        }

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
        public async Task<IActionResult> GetById(Guid id, string? detailLevel)
        {
            var coupon = await _couponService.GetByIdAsync(id, detailLevel);
            if (coupon == null)
            {
                return NotFound(new ApiResponse(404, $"Coupon with id {id} not found"));
            }
            return Ok(coupon);
        }

        [HttpPost]
        public async Task<IActionResult> Create(CouponCreateDTO couponDTO)
        {
            var validationResult = _couponValidator.Validate(couponDTO);
            ValidationHandler.HandleValidationResult(ModelState, validationResult);

            var (guid, basicDto) = await _couponService.AddAsync(couponDTO);
            return CreatedAtAction(nameof(GetById), new { id = guid }, basicDto);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, CouponUpdateDTO couponDTO)
        {
            var validationResult = _couponValidator.Validate(couponDTO);
            ValidationHandler.HandleValidationResult(ModelState, validationResult);

            var existingCoupon = await _couponService.GetByIdAsync(id);
            if (existingCoupon == null)
            {
                return NotFound(new ApiResponse(404, $"Coupon with id {id} not found"));
            }

            await _couponService.UpdateAsync(couponDTO);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var coupon = (CouponWithIdDTO) await _couponService.GetByIdAsync(id, "WithId");
            if (coupon == null)
            {
                return NotFound(new ApiResponse(404, $"Coupon with id {id} not found"));
            }

            await _couponService.DeleteAsync(coupon);
            return NoContent();
        }
    }
}
