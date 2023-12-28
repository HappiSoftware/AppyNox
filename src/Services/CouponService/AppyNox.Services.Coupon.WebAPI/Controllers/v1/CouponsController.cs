﻿using AppyNox.Services.Base.API.Helpers;
using AppyNox.Services.Base.API.ViewModels;
using AppyNox.Services.Base.Application.ExceptionExtensions;
using AppyNox.Services.Base.Application.Services.Implementations;
using AppyNox.Services.Base.Application.Services.Interfaces;
using AppyNox.Services.Base.Infrastructure.Interfaces;
using AppyNox.Services.Base.Infrastructure.Repositories.Common;
using AppyNox.Services.Coupon.Application.Dtos.CouponDtos.Models.Base;
using AppyNox.Services.Coupon.Application.Services.Implementations;
using AppyNox.Services.Coupon.Application.Services.Interfaces;
using AppyNox.Services.Coupon.Domain.Entities;
using AppyNox.Services.Coupon.WebAPI.Helpers.Permissions;
using Asp.Versioning;
using AutoWrapper.Extensions;
using AutoWrapper.Wrappers;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Text.Json;

namespace AppyNox.Services.Coupon.WebAPI.Controllers.v1
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/[controller]")]
    public class CouponsController(IGenericService<CouponEntity> couponService) : Controller
    {
        #region [ Fields ]

        private readonly IGenericService<CouponEntity> _couponService = couponService;

        #endregion

        #region [ Public Methods ]

        [HttpGet]
        [Authorize(Permissions.Coupons.View)]
        public async Task<ApiResponse> GetAll([FromQuery] QueryParametersViewModel queryParameters)
        {
            var coupons = await _couponService.GetAllAsync(queryParameters);
            return new ApiResponse(coupons);
        }

        [HttpGet("{id}")]
        [Authorize(Permissions.Coupons.View)]
        public async Task<ApiResponse> GetById(Guid id, [FromQuery] QueryParametersViewModel queryParameters)
        {
            var coupon = await _couponService.GetByIdAsync(id, queryParameters);
            return new ApiResponse(coupon);
        }

        [HttpPost]
        [Authorize(Permissions.Coupons.Create)]
        public async Task<IActionResult> Create([FromBody] dynamic couponDto, string detailLevel = "Simple")
        {
            var result = await _couponService.AddAsync(couponDto, detailLevel);
            return CreatedAtAction(nameof(GetById), new { id = result.Item1 }, result.Item2);
        }

        [HttpPut("{id}")]
        [Authorize(Permissions.Coupons.Edit)]
        public async Task<IActionResult> Update(Guid id, [FromBody] dynamic couponDto, string detailLevel = "Simple")
        {
            if (id != ValidationHelpers.GetIdFromDynamicDto(couponDto))
            {
                return BadRequest();
            }

            await _couponService.GetByIdAsync(id, QueryParameters.CreateForIdOnly());

            await _couponService.UpdateAsync(couponDto, detailLevel);
            return NoContent();
        }

        [HttpDelete("{id}")]
        [Authorize(Permissions.Coupons.Delete)]
        public async Task<IActionResult> Delete(Guid id)
        {
            var coupon = await _couponService.GetByIdAsync(id, QueryParameters.CreateForIdOnly());
            if (coupon == null)
            {
                return NotFound(new ApiResponse(404, $"Coupon with id {id} not found"));
            }

            var couponPropertiesDictionary = (IDictionary<string, object>)coupon;
            Guid Id = (Guid)couponPropertiesDictionary["Id"];
            await _couponService.DeleteAsync(Id);
            return NoContent();
        }

        #endregion
    }
}