using AppyNox.Services.Base.API.Helpers;
using AppyNox.Services.Base.API.ViewModels;
using AppyNox.Services.Base.Application.ExceptionExtensions;
using AppyNox.Services.Base.Application.Services.Interfaces;
using AppyNox.Services.Base.Infrastructure.Repositories.Common;
using AppyNox.Services.Coupon.Application.Dtos.CouponDtos.Models.Base;
using AppyNox.Services.Coupon.Application.Services.Implementations;
using AppyNox.Services.Coupon.Application.Services.Interfaces;
using AppyNox.Services.Coupon.Domain.Entities;
using Asp.Versioning;
using AutoWrapper.Extensions;
using AutoWrapper.Wrappers;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace AppyNox.Services.Coupon.WebAPI.Controllers.v1
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/[controller]")]
    public class CouponsController : Controller
    {
        #region [ Fields ]

        private readonly IGenericService<CouponEntity> _couponService;

        #endregion

        #region [ Public Constructors ]

        public CouponsController(IGenericService<CouponEntity> couponService)
        {
            _couponService = couponService;
        }

        #endregion

        #region [ Public Methods ]

        [HttpGet]
        public async Task<ApiResponse> GetAll([FromQuery] QueryParametersViewModel queryParameters)
        {
            var coupons = await _couponService.GetAllAsync(queryParameters);
            return new ApiResponse(coupons);
        }

        [HttpGet("{id}")]
        public async Task<ApiResponse> GetById(Guid id, [FromQuery] QueryParametersViewModel queryParameters)
        {
            var coupon = await _couponService.GetByIdAsync(id, queryParameters);
            return new ApiResponse(coupon);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] dynamic couponDto, string detailLevel = "Simple")
        {
            var result = await _couponService.AddAsync(couponDto, detailLevel);
            return CreatedAtAction(nameof(GetById), new { id = result.Item1 }, result.Item2);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] CouponSimpleUpdateDto couponDto)
        {
            if (id != couponDto.Id)
            {
                return BadRequest();
            }

            await _couponService.GetByIdAsync(id, QueryParameters.CreateForIdOnly());

            await _couponService.UpdateAsync(couponDto);
            return NoContent();
        }

        [HttpDelete("{id}")]
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