﻿using AppyNox.Services.Authentication.Application.Dtos.ClaimDtos.Models.Base;
using AppyNox.Services.Authentication.Application.Dtos.IdentityRoleDtos.Models.Base;
using AppyNox.Services.Authentication.Application.Dtos.IdentityRoleDtos.Models.Extended;
using AppyNox.Services.Authentication.WebAPI.Filters;
using AppyNox.Services.Base.API.ViewModels;
using Asp.Versioning;
using AutoMapper;
using AutoWrapper.Wrappers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using static AppyNox.Services.Authentication.WebAPI.Utilities.Permissions;

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
        public async Task<ApiResponse> GetAll([FromQuery] QueryParametersViewModel queryParameters)
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
        public async Task<ApiResponse> GetById(string id, [FromQuery] QueryParametersViewModel queryParameters)
        {
            var identityRole = await _roleManager.FindByIdAsync(id.ToString());

            if (identityRole == null)
            {
                throw new ApiProblemDetailsException("Not Found", 404);
            }
            return new ApiResponse(_mapper.Map(identityRole, identityRole.GetType(), typeof(IdentityRoleDto)));
        }

        [HttpPut("{id}")]
        [Authorize(Roles.Edit)]
        public async Task<IActionResult> Put(string id, [FromBody] IdentityRoleUpdateDto identityRoleUpdateDto)
        {
            if (id != identityRoleUpdateDto.Id)
            {
                throw new ApiProblemDetailsException("Ids don't match", 422);
            }

            var existingRole = await _roleManager.FindByIdAsync(id);
            if (existingRole == null)
            {
                throw new ApiProblemDetailsException("Role Not Found", 404);
            }

            var concurrencyStamp = existingRole.ConcurrencyStamp;
            existingRole.Name = identityRoleUpdateDto.Name;

            var result = await _roleValidator.ValidateAsync(_roleManager, existingRole);
            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(error.Code, error.Description);
                }
                throw new ApiProblemDetailsException(ModelState);
            }

            try
            {
                existingRole.ConcurrencyStamp = concurrencyStamp;

                IdentityResult identityResult = await _roleManager.UpdateAsync(existingRole);
                if (!identityResult.Succeeded)
                {
                    foreach (var item in identityResult.Errors)
                    {
                        ModelState.AddModelError(item.Code, item.Description);
                    }
                    throw new ApiProblemDetailsException(ModelState);
                }
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await IdentityRoleExists(id))
                {
                    throw new ApiProblemDetailsException(404);
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
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(error.Code, error.Description);
                }
                throw new ApiProblemDetailsException(ModelState);
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
        public async Task<IActionResult> Delete(string id)
        {
            var identityRole = await _roleManager.FindByIdAsync(id.ToString());
            if (identityRole == null)
            {
                throw new ApiProblemDetailsException("Not Found", 404);
            }

            await _roleManager.DeleteAsync(identityRole);

            return NoContent();
        }

        #endregion

        #region [ Role-Claim Operations ]

        [HttpGet]
        [Authorize(Roles.View)]
        [Route("/api/roles/{rid}/claims")]
        public async Task<ApiResponse> GetClaims(string rid)
        {
            List<string> parameterList = new List<string> { "Value1", "Value2" };
            HttpContext.Items["RouteParameters"] = parameterList;

            var role = await _roleManager.FindByIdAsync(rid);
            if (role == null)
                throw new ApiProblemDetailsException($"Record with id: {rid} does not exist.", 404);
            IList<ClaimDto> claims = _mapper.Map<List<ClaimDto>>(await _roleManager.GetClaimsAsync(role));

            var roleWithClaimsDto = _mapper.Map<IdentityRoleWithClaimsDto>(role);
            roleWithClaimsDto.Claims = claims;

            return new ApiResponse(roleWithClaimsDto, 200);
        }

        [HttpPost]
        [Authorize(Roles.AssignPermission)]
        [Route("/api/Roles/{rid}/Claims")]
        public async Task<ApiResponse> AssignClaim(string rid, [FromBody] ClaimDto claim)
        {
            var role = await _roleManager.FindByIdAsync(rid);
            if (role == null)
                throw new ApiProblemDetailsException($"Record with id: {rid} does not exist.", 404);

            //Assign claim to role
            IList<ClaimDto> claims = _mapper.Map<List<ClaimDto>>(await _roleManager.GetClaimsAsync(role));

            //Manual check if role has this claim
            if (claims.Any(x => x.Value == claim.Value && x.Type == claim.Type))
                throw new ApiProblemDetailsException($"Role with id {rid} already has this claim", 409);

            IdentityResult response = await _roleManager.AddClaimAsync(role, new Claim(claim.Type, claim.Value));

            if (!response.Succeeded)
            {
                foreach (var error in response.Errors)
                {
                    ModelState.AddModelError(error.Code, error.Description);
                }
                throw new ApiProblemDetailsException(ModelState);
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
        public async Task<IActionResult> WithdrawClaim(string rid, string claimValue)
        {
            var role = await _roleManager.FindByIdAsync(rid);
            if (role == null)
                throw new ApiProblemDetailsException($"Record with id: {rid} does not exist.", 404);

            IdentityResult response = await _roleManager.RemoveClaimAsync(role, new Claim("API.Permission", claimValue));

            if (!response.Succeeded)
            {
                foreach (var error in response.Errors)
                {
                    ModelState.AddModelError(error.Code, error.Description);
                }
                throw new ApiProblemDetailsException(ModelState);
            }
            return NoContent();
        }

        #endregion

        #region [ Private Methods ]

        private async Task<bool> IdentityRoleExists(string id)
        {
            return await _roleManager.RoleExistsAsync(id);
        }

        #endregion
    }
}