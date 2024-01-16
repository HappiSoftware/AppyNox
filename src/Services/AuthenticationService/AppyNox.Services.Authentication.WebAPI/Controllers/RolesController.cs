﻿using AppyNox.Services.Authentication.Application.Dtos.ClaimDtos.Models.Base;
using AppyNox.Services.Authentication.Application.Dtos.IdentityRoleDtos.Models.Base;
using AppyNox.Services.Authentication.Application.Dtos.IdentityRoleDtos.Models.Extended;
using AppyNox.Services.Authentication.WebAPI.ExceptionExtensions.Base;
using AppyNox.Services.Authentication.WebAPI.Filters;
using AppyNox.Services.Base.Application.ExceptionExtensions;
using Asp.Versioning;
using AutoMapper;
using AutoWrapper.Wrappers;
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
    [Route("api/[controller]")]
    [ApiVersion("1.0")]
    [ApiController]
    [JwtTokenValidate]
    public class RolesController : ControllerBase
    {
        #region [ Fields ]

        private readonly RoleManager<IdentityRole> _roleManager;

        private readonly IRoleValidator<IdentityRole> _roleValidator;

        private readonly IMapper _mapper;

        #endregion

        #region [ Public Constructors ]

        public RolesController(IMapper mapper, RoleManager<IdentityRole> roleManager,
            IRoleValidator<IdentityRole> roleValidator)
        {
            _mapper = mapper;
            _roleManager = roleManager;
            _roleValidator = roleValidator;
        }

        #endregion

        #region [ CRUD Operations ]

        [HttpGet]
        [Authorize(Roles.View)]
        public async Task<ApiResponse> GetAll()
        {
            var entities = await _roleManager.Roles.ToListAsync();
            object response = new
            {
                count = _roleManager.Roles.Count().ToString(),
                roles = _mapper.Map(entities, entities.GetType(), typeof(List<IdentityRoleDto>))
            };

            return new ApiResponse(response);
        }

        [HttpGet("{id}")]
        [Authorize(Roles.View)]
        public async Task<ApiResponse> GetById(Guid id)
        {
            var identityRole = await _roleManager.FindByIdAsync(id.ToString());

            if (identityRole == null)
            {
                throw new AuthenticationServiceException("Not Found", (int)HttpStatusCode.NotFound);
            }
            return new ApiResponse(_mapper.Map(identityRole, identityRole.GetType(), typeof(IdentityRoleDto)));
        }

        [HttpPut("{id}")]
        [Authorize(Roles.Edit)]
        public async Task<IActionResult> Put(Guid id, [FromBody] IdentityRoleUpdateDto identityRoleUpdateDto)
        {
            if (id.ToString() != identityRoleUpdateDto.Id)
            {
                throw new AuthenticationServiceException("Ids don't match", (int)HttpStatusCode.UnprocessableContent);
            }

            var existingRole = await _roleManager.FindByIdAsync(id.ToString());
            if (existingRole == null)
            {
                throw new AuthenticationServiceException("Role Not Found", (int)HttpStatusCode.NotFound);
            }

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
                throw new FluentValidationException(typeof(IdentityRole), validationResult);
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
                    throw new FluentValidationException(typeof(IdentityRole), validationResult);
                }
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await IdentityRoleExists(id))
                {
                    throw new AuthenticationServiceException("Role Not Found", (int)HttpStatusCode.NotFound);
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
        public async Task<ApiResponse> Post(IdentityRoleCreateDto identityRoleDto)
        {
            var roleEntity = _mapper.Map<IdentityRole>(identityRoleDto);
            var result = await _roleValidator.ValidateAsync(_roleManager, roleEntity);
            if (!result.Succeeded)
            {
                ValidationResult validationResult = new();
                foreach (var error in result.Errors)
                {
                    ValidationFailure validationFailure = new(error.Code, error.Description);
                    validationResult.Errors.Add(validationFailure);
                }
                throw new FluentValidationException(typeof(IdentityRole), validationResult);
            }

            await _roleManager.CreateAsync(roleEntity);

            object response = new
            {
                id = roleEntity.Id,
                value = identityRoleDto
            };

            return new ApiResponse("New record has been created in the database.", response, 201);
        }

        [HttpDelete("{id}")]
        [Authorize(Roles.Delete)]
        public async Task<IActionResult> Delete(Guid id)
        {
            var identityRole = await _roleManager.FindByIdAsync(id.ToString());
            if (identityRole == null)
            {
                throw new AuthenticationServiceException("Role Not Found", (int)HttpStatusCode.NotFound);
            }

            await _roleManager.DeleteAsync(identityRole);

            return NoContent();
        }

        #endregion

        #region [ Role-Claim Operations ]

        [HttpGet]
        [Authorize(Roles.View)]
        [Route("/api/roles/{rid}/claims")]
        public async Task<ApiResponse> GetClaims(Guid rid)
        {
            List<string> parameterList = new List<string> { "Value1", "Value2" };
            HttpContext.Items["RouteParameters"] = parameterList;

            var role = await _roleManager.FindByIdAsync(rid.ToString());
            if (role == null)
                throw new AuthenticationServiceException($"Record with id: {rid} does not exist.", (int)HttpStatusCode.NotFound);
            IList<ClaimDto> claims = _mapper.Map<List<ClaimDto>>(await _roleManager.GetClaimsAsync(role));

            var roleWithClaimsDto = _mapper.Map<IdentityRoleWithClaimsDto>(role);
            roleWithClaimsDto.Claims = claims;

            return new ApiResponse(roleWithClaimsDto, 200);
        }

        [HttpPost]
        [Authorize(Roles.AssignPermission)]
        [Route("/api/Roles/{rid}/Claims")]
        public async Task<ApiResponse> AssignClaim(Guid rid, [FromBody] ClaimDto claim)
        {
            var role = await _roleManager.FindByIdAsync(rid.ToString());
            if (role == null)
                throw new AuthenticationServiceException($"Record with id: {rid} does not exist.", (int)HttpStatusCode.NotFound);

            //Assign claim to role
            IList<ClaimDto> claims = _mapper.Map<List<ClaimDto>>(await _roleManager.GetClaimsAsync(role));

            //Manual check if role has this claim
            if (claims.Any(x => x.Value == claim.Value && x.Type == claim.Type))
                throw new AuthenticationServiceException($"Role with id {rid} already has this claim", (int)HttpStatusCode.Conflict);

            IdentityResult response = await _roleManager.AddClaimAsync(role, new Claim(claim.Type, claim.Value));

            if (!response.Succeeded)
            {
                ValidationResult validationResult = new();
                foreach (var error in response.Errors)
                {
                    ValidationFailure validationFailure = new(error.Code, error.Description);
                    validationResult.Errors.Add(validationFailure);
                }
                throw new FluentValidationException(typeof(IdentityRole), validationResult);
            }

            //Refill claims list to send it as response
            claims = _mapper.Map<List<ClaimDto>>(await _roleManager.GetClaimsAsync(role));

            var roleWithClaimsDto = _mapper.Map<IdentityRoleWithClaimsDto>(role);
            roleWithClaimsDto.Claims = claims;

            return new ApiResponse(roleWithClaimsDto, 200);
        }

        [HttpDelete]
        [Authorize(Roles.WithdrawPermission)]
        [Route("/api/Roles/{rid}/Claims/{claimValue}")]
        public async Task<IActionResult> WithdrawClaim(Guid rid, string claimValue)
        {
            var role = await _roleManager.FindByIdAsync(rid.ToString());
            if (role == null)
                throw new AuthenticationServiceException($"Record with id: {rid} does not exist.", (int)HttpStatusCode.NotFound);

            IdentityResult response = await _roleManager.RemoveClaimAsync(role, new Claim("API.Permission", claimValue));

            if (!response.Succeeded)
            {
                ValidationResult validationResult = new();
                foreach (var error in response.Errors)
                {
                    ValidationFailure validationFailure = new(error.Code, error.Description);
                    validationResult.Errors.Add(validationFailure);
                }
                throw new FluentValidationException(typeof(IdentityRole), validationResult);
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