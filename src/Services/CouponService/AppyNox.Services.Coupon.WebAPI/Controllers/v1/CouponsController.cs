﻿using AppyNox.Services.Coupon.Application.Dtos.CouponDtos.Models.Base;
using AppyNox.Services.Coupon.Application.ExceptionExtensions;
using AppyNox.Services.Coupon.Application.Services.Implementations;
using AppyNox.Services.Coupon.Application.Services.Interfaces;
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

        private readonly IGenericService<CouponEntity, CouponSimpleDto, CouponBasicCreateDto, CouponSimpleUpdateDto> _couponService;

        private readonly IValidator<CouponBasicCreateDto> _couponValidator;

        #endregion

        #region [ Public Constructors ]

        public CouponsController(
            IGenericService<CouponEntity, CouponSimpleDto, CouponBasicCreateDto, CouponSimpleUpdateDto> couponService,
            IValidator<CouponBasicCreateDto> couponValidator)
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
                var coupons = await _couponService.GetAllAsync(queryParameters, queryParameters.DetailLevel);
                return new ApiResponse(coupons, 200);
            }
            catch (DetailLevelNotFoundException exception)
            {
                throw new ApiProblemDetailsException(exception.Message, statusCode: 400);
            }
        }

        [HttpGet("{id}")]
        public async Task<ApiResponse> GetById(Guid id, [FromQuery] QueryParameters queryParameters)
        {
            var coupon = await _couponService.GetByIdAsync(id, queryParameters);
            return coupon == null ? throw new ApiProblemDetailsException($"Record with id: {id} does not exist.", 404) : new ApiResponse(coupon);
        }

        [HttpPost]
        public async Task<IActionResult> Create(CouponBasicCreateDto couponDto)
        {
            var validationResult = _couponValidator.Validate(couponDto);
            ValidationHandler.HandleValidationResult(ModelState, validationResult);

            var (guid, basicDto) = await _couponService.AddAsync(couponDto);
            return CreatedAtAction(nameof(GetById), new { id = guid }, basicDto);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, CouponSimpleUpdateDto couponDto)
        {
            //var validationResult = _couponValidator.Validate(couponDto);
            //ValidationHandler.HandleValidationResult(ModelState, validationResult);

            //var existingCoupon = await _couponService.GetByIdAsync(id);
            //if (existingCoupon == null)
            //{
            //    return NotFound(new ApiResponse(404, $"Coupon with id {id} not found"));
            //}

            //await _couponService.UpdateAsync(couponDto);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            //var coupon = await _couponService.GetByIdAsync(id, "WithId");
            //if (coupon == null)
            //{
            //    return NotFound(new ApiResponse(404, $"Coupon with id {id} not found"));
            //}

            //await _couponService.DeleteAsync(coupon);
            return NoContent();
        }

        #endregion
    }
}