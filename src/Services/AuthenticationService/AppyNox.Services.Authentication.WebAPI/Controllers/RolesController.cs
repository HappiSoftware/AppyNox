using AppyNox.Services.Authentication.Application.DTOs.ApplicationRoleDTOs.Models;
using AppyNox.Services.Authentication.Application.DTOs.ClaimDtos.Models;
using AppyNox.Services.Authentication.Application.Validators.ApplicationRoleValidators;
using AppyNox.Services.Authentication.Domain.Entities;
using AppyNox.Services.Authentication.Infrastructure.AsyncLocals;
using AppyNox.Services.Authentication.WebAPI.ExceptionExtensions.Base;
using AppyNox.Services.Authentication.WebAPI.Extensions;
using AppyNox.Services.Authentication.WebAPI.Localization;
using AppyNox.Services.Base.API.Localization;
using AppyNox.Services.Base.API.Wrappers;
using AppyNox.Services.Base.Application.ExceptionExtensions;
using AppyNox.Services.Base.Core.Extensions;
using Asp.Versioning;
using AutoMapper;
using FluentValidation.Results;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net;
using System.Security.Claims;
using static AppyNox.Services.Authentication.WebAPI.Permission.Permissions;

namespace AppyNox.Services.Authentication.WebAPI.Controllers
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class RolesController(IMapper mapper, RoleManager<ApplicationRole> roleManager,
        IRoleValidator<ApplicationRole> roleValidator, ApplicationRoleCreateDtoValidator roleDtoCreateValidator)
        : ControllerBase
    {
        #region [ Fields ]

        private readonly RoleManager<ApplicationRole> _roleManager = roleManager;

        private readonly IRoleValidator<ApplicationRole> _roleValidator = roleValidator;

        private readonly ApplicationRoleCreateDtoValidator _roleDtoCreateValidator = roleDtoCreateValidator;

        private readonly IMapper _mapper = mapper;

        #endregion

        #region [ CRUD Operations ]

        [HttpGet]
        [Authorize(Roles.View)]
        public async Task<IActionResult> GetAll()
        {
            var entities = await _roleManager.Roles.ToListAsync();
            object response = new
            {
                count = _roleManager.Roles.Count().ToString(),
                roles = _mapper.Map(entities, entities.GetType(), typeof(List<ApplicationRoleDto>))
            };

            return Ok(response);
        }

        [HttpGet("{id}")]
        [Authorize(Roles.View)]
        public async Task<IActionResult> GetById(Guid id)
        {
            var identityRole = await _roleManager.FindByIdAsync(id.ToString())
                ?? throw new NoxSsoApiException("Not Found", (int)HttpStatusCode.NotFound);

            IList<ClaimDto> claims = _mapper.Map<List<ClaimDto>>(await _roleManager.GetClaimsAsync(identityRole));

            var roleWithClaimsDto = _mapper.Map<ApplicationRoleWithClaimsDto>(identityRole);
            roleWithClaimsDto.Claims = claims;

            return Ok(roleWithClaimsDto);
        }

        [HttpPut("{id}")]
        [Authorize(Roles.Edit)]
        public async Task<IActionResult> Put(Guid id, [FromBody] ApplicationRoleUpdateDto identityRoleUpdateDto)
        {
            if (id != identityRoleUpdateDto.Id)
            {
                throw new NoxSsoApiException(NoxApiResourceService.IdMismatch, statusCode: (int)HttpStatusCode.UnprocessableContent);
            }

            var existingRole = await _roleManager.FindByIdAsync(id.ToString())
                ?? throw new NoxSsoApiException(NoxSsoApiResourceService.RoleNotFound, statusCode: (int)HttpStatusCode.NotFound);
            var concurrencyStamp = existingRole.ConcurrencyStamp;
            existingRole.Name = identityRoleUpdateDto.Name;

            var result = await _roleValidator.ValidateAsync(_roleManager, existingRole);
            if (!result.Succeeded)
            {
                ValidationResult validationResult = new();
                foreach (var error in result.Errors)
                {
                    ValidationFailure validationFailure = new(error.Code, error.Description);
                    validationResult.Errors.Add(validationFailure);
                }
                throw new FluentValidationException(typeof(ApplicationRole), validationResult);
            }

            try
            {
                existingRole.ConcurrencyStamp = concurrencyStamp;

                IdentityResult identityResult = await _roleManager.UpdateAsync(existingRole);
                if (!identityResult.Succeeded)
                {
                    ValidationResult validationResult = new();
                    foreach (var error in identityResult.Errors)
                    {
                        ValidationFailure validationFailure = new(error.Code, error.Description);
                        validationResult.Errors.Add(validationFailure);
                    }
                    throw new FluentValidationException(typeof(ApplicationRole), validationResult);
                }
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await IdentityRoleExists(id))
                {
                    throw new NoxSsoApiException(NoxSsoApiResourceService.RoleNotFound, statusCode: (int)HttpStatusCode.NotFound);
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        [HttpPost]
        [Authorize(Roles.Create)]
        public async Task<IActionResult> Post(ApplicationRoleCreateDto identityRoleDto)
        {
            ValidationResult fluentValidationResult = _roleDtoCreateValidator.Validate(identityRoleDto);
            if (!fluentValidationResult.IsValid)
            {
                throw new FluentValidationException(typeof(ApplicationRoleCreateDto), fluentValidationResult);
            }

            var roleEntity = _mapper.Map<ApplicationRole>(identityRoleDto);
            IdentityResult result = await _roleValidator.ValidateAsync(_roleManager, roleEntity);
            result.HandleValidationResult();

            roleEntity.CompanyId = AuthenticationContext.CompanyId;
            await _roleManager.CreateAsync(roleEntity);

            object response = new
            {
                id = roleEntity.Id,
                value = identityRoleDto
            };

            return CreatedAtAction(nameof(GetById), response);
        }

        [HttpDelete("{id}")]
        [Authorize(Roles.Delete)]
        public async Task<IActionResult> Delete(Guid id)
        {
            var identityRole = await _roleManager.FindByIdAsync(id.ToString())
                ?? throw new NoxSsoApiException(NoxSsoApiResourceService.RoleNotFound, statusCode: (int)HttpStatusCode.NotFound);

            await _roleManager.DeleteAsync(identityRole);

            return NoContent();
        }

        #endregion

        #region [ Role-Claim Operations ]

        [HttpGet]
        [Authorize(Roles.View)]
        [Route("/api/roles/{rid}/claims")]
        public async Task<IActionResult> GetClaims(Guid rid)
        {
            List<string> parameterList = ["Value1", "Value2"];
            HttpContext.Items["RouteParameters"] = parameterList;

            var role = await _roleManager.FindByIdAsync(rid.ToString())
                ?? throw new NoxSsoApiException(NoxSsoApiResourceService.RecordNotFound.Format(rid), statusCode: (int)HttpStatusCode.NotFound);

            IList<ClaimDto> claims = _mapper.Map<List<ClaimDto>>(await _roleManager.GetClaimsAsync(role));

            var roleWithClaimsDto = _mapper.Map<ApplicationRoleWithClaimsDto>(role);
            roleWithClaimsDto.Claims = claims;

            return Ok(roleWithClaimsDto);
        }

        [HttpPost]
        [Authorize(Roles.AssignPermission)]
        [Route("/api/Roles/{rid}/Claims")]
        public async Task<IActionResult> AssignClaim(Guid rid, [FromBody] ClaimDto claim)
        {
            var role = await _roleManager.FindByIdAsync(rid.ToString())
                ?? throw new NoxSsoApiException(NoxSsoApiResourceService.RecordNotFound.Format(rid), statusCode: (int)HttpStatusCode.NotFound);

            //Assign claim to role
            IList<ClaimDto> claims = _mapper.Map<List<ClaimDto>>(await _roleManager.GetClaimsAsync(role));

            //Manual check if role has this claim
            if (claims.Any(x => x.Value == claim.Value && x.Type == claim.Type))
                throw new NoxSsoApiException(NoxSsoApiResourceService.AlreadyHasClaim.Format(rid), statusCode: (int)HttpStatusCode.Conflict);

            IdentityResult response = await _roleManager.AddClaimAsync(role, new Claim(claim.Type, claim.Value));

            if (!response.Succeeded)
            {
                ValidationResult validationResult = new();
                foreach (var error in response.Errors)
                {
                    ValidationFailure validationFailure = new(error.Code, error.Description);
                    validationResult.Errors.Add(validationFailure);
                }
                throw new FluentValidationException(typeof(ApplicationRole), validationResult);
            }

            //Refill claims list to send it as response
            claims = _mapper.Map<List<ClaimDto>>(await _roleManager.GetClaimsAsync(role));

            var roleWithClaimsDto = _mapper.Map<ApplicationRoleWithClaimsDto>(role);
            roleWithClaimsDto.Claims = claims;

            return Ok(roleWithClaimsDto);
        }

        [HttpDelete]
        [Authorize(Roles.WithdrawPermission)]
        [Route("/api/Roles/{rid}/Claims/{claimValue}")]
        public async Task<IActionResult> WithdrawClaim(Guid rid, string claimValue)
        {
            var role = await _roleManager.FindByIdAsync(rid.ToString())
                ?? throw new NoxSsoApiException(NoxSsoApiResourceService.RecordNotFound.Format(rid), statusCode: (int)HttpStatusCode.NotFound);

            IdentityResult response = await _roleManager.RemoveClaimAsync(role, new Claim("Permission", claimValue));

            if (!response.Succeeded)
            {
                ValidationResult validationResult = new();
                foreach (var error in response.Errors)
                {
                    ValidationFailure validationFailure = new(error.Code, error.Description);
                    validationResult.Errors.Add(validationFailure);
                }
                throw new FluentValidationException(typeof(ApplicationRole), validationResult);
            }
            return NoContent();
        }

        #endregion

        #region [ Private Methods ]

        private async Task<bool> IdentityRoleExists(Guid id)
        {
            return await _roleManager.RoleExistsAsync(id.ToString());
        }

        #endregion
    }
}