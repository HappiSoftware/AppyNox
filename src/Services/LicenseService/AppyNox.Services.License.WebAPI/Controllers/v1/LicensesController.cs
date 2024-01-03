using AppyNox.Services.Base.API.Helpers;
using AppyNox.Services.Base.API.ViewModels;
using AppyNox.Services.Base.Infrastructure.Repositories.Common;
using AppyNox.Services.License.Application.Services.Interfaces;
using AppyNox.Services.License.Domain.Entities;
using AppyNox.Services.License.WebAPI.Helpers.Permissions;
using Asp.Versioning;
using AutoWrapper.Wrappers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AppyNox.Services.Coupon.WebAPI.Controllers.v1
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/[controller]")]
    public class LicensesController(IGenericService<LicenseEntity> licenseService) : Controller
    {
        #region [ Fields ]

        private readonly IGenericService<LicenseEntity> _licenseService = licenseService;

        #endregion

        #region [ Public Methods ]

        [HttpGet]
        [Authorize(Permissions.Licenses.View)]
        public async Task<ApiResponse> GetAll([FromQuery] QueryParametersViewModel queryParameters)
        {
            var licenses = await _licenseService.GetAllAsync(queryParameters);
            return new ApiResponse(licenses);
        }

        [HttpGet("{id}")]
        [Authorize(Permissions.Licenses.View)]
        public async Task<ApiResponse> GetById(Guid id, [FromQuery] QueryParametersViewModel queryParameters)
        {
            var license = await _licenseService.GetByIdAsync(id, queryParameters);
            return new ApiResponse(license);
        }

        [HttpPost]
        [Authorize(Permissions.Licenses.Create)]
        public async Task<IActionResult> Create([FromBody] dynamic licenseDto, string detailLevel = "Simple")
        {
            var result = await _licenseService.AddAsync(licenseDto, detailLevel);
            return CreatedAtAction(nameof(GetById), new { id = result.Item1 }, result.Item2);
        }

        [HttpPut("{id}")]
        [Authorize(Permissions.Licenses.Edit)]
        public async Task<IActionResult> Update(Guid id, [FromBody] dynamic licenseDto, string detailLevel = "Simple")
        {
            if (id != ValidationHelpers.GetIdFromDynamicDto(licenseDto))
            {
                return BadRequest();
            }

            await _licenseService.GetByIdAsync(id, QueryParameters.CreateForIdOnly());

            await _licenseService.UpdateAsync(licenseDto, detailLevel);
            return NoContent();
        }

        [HttpDelete("{id}")]
        [Authorize(Permissions.Licenses.Delete)]
        public async Task<IActionResult> Delete(Guid id)
        {
            var license = await _licenseService.GetByIdAsync(id, QueryParameters.CreateForIdOnly());
            if (license == null)
            {
                return NotFound(new ApiResponse(404, $"Coupon with id {id} not found"));
            }

            var licensePropertiesDictionary = (IDictionary<string, object>)license;
            Guid Id = (Guid)licensePropertiesDictionary["Id"];
            await _licenseService.DeleteAsync(Id);
            return NoContent();
        }

        #endregion
    }
}