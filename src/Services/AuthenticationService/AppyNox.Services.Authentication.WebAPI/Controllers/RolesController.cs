using AppyNox.Services.Authentication.Application.DTOs.ClaimDTOs;
using AppyNox.Services.Authentication.Application.DTOs.IdentityRoleDTOs.Models;
using AppyNox.Services.Authentication.WebAPI.Filters;
using AppyNox.Services.Authentication.WebAPI.Helpers;
using AppyNox.Services.Authentication.WebAPI.Utilities;
using Asp.Versioning;
using AutoMapper;
using AutoWrapper.Wrappers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Security;
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
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IRoleValidator<IdentityRole> _roleValidator;
        private readonly IMapper _mapper;
        private readonly DtoMappingHelper<IdentityRole> _dtoMappingHelper;

        public RolesController(IMapper mapper, RoleManager<IdentityRole> roleManager,
            IRoleValidator<IdentityRole> roleValidator, DtoMappingHelper<IdentityRole> dtoMappingHelper)
        {
            _mapper = mapper;
            _roleManager = roleManager;
            _roleValidator = roleValidator;
            _dtoMappingHelper = dtoMappingHelper;
        }

        #region [ CRUD Operations ]

        [HttpGet]
        [Authorize(Roles.View)]
        public async Task<ApiResponse> GetAll([FromQuery] string? detailLevel)
        {
            var entities = await _roleManager.Roles.ToListAsync();
            object response = new
            {
                count = _roleManager.Roles.Count().ToString(),
                roles = _mapper.Map(entities, entities.GetType(), typeof(IEnumerable<>).MakeGenericType(_dtoMappingHelper.GetLeveledDtoType(detailLevel)))
            };

            return new ApiResponse(response);
        }

        [HttpGet("{id}")]
        [Authorize(Roles.View)]
        [GuidCheckFilter]
        public async Task<ApiResponse> GetById(string id, [FromQuery] string? detailLevel)
        {
            var identityRole = await _roleManager.FindByIdAsync(id.ToString());

            if (identityRole == null)
            {
                throw new ApiProblemDetailsException("Not Found", 404);
            }
            return new ApiResponse(_mapper.Map(identityRole, identityRole.GetType(), _dtoMappingHelper.GetLeveledDtoType(detailLevel)));
        }

        [HttpPut("{id}")]
        [Authorize(Roles.Edit)]
        [GuidCheckFilter]
        public async Task<IActionResult> Put(string id, [FromBody] IdentityRoleUpdateDTO identityRoleUpdateDTO)
        {
            if (id != identityRoleUpdateDTO.Id)
            {
                throw new ApiProblemDetailsException("Ids don't match", 422);
            }

            var existingRole = await _roleManager.FindByIdAsync(id);
            if (existingRole == null)
            {
                throw new ApiProblemDetailsException("Role Not Found", 404);
            }

            var concurrencyStamp = existingRole.ConcurrencyStamp;
            existingRole.Name = identityRoleUpdateDTO.Name;

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
        public async Task<ApiResponse> Post(IdentityRoleCreateDTO identityRoleDTO)
        {
            var roleEntity = _mapper.Map<IdentityRole>(identityRoleDTO);
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
                value = identityRoleDTO
            };

            return new ApiResponse("New record has been created in the database.", response, 201);
        }

        [HttpDelete("{id}")]
        [Authorize(Roles.Delete)]
        [GuidCheckFilter]
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
        [GuidCheckFilter(new string[] { "rid" })]
        public async Task<ApiResponse> GetClaims(string rid)
        {
            List<string> parameterList = new List<string> { "Value1", "Value2" };
            HttpContext.Items["RouteParameters"] = parameterList;

            var role = await _roleManager.FindByIdAsync(rid);
            if (role == null)
                throw new ApiProblemDetailsException($"Record with id: {rid} does not exist.", 404);
            IList<ClaimDTO> claims = _mapper.Map<List<ClaimDTO>>(await _roleManager.GetClaimsAsync(role));

            var roleWithClaimsDto = _mapper.Map<IdentityRoleWithClaimsDTO>(role);
            roleWithClaimsDto.Claims = claims;

            return new ApiResponse(roleWithClaimsDto, 200);
        }

        [HttpPost]
        [Authorize(Roles.AssignPermission)]
        [Route("/api/Roles/{rid}/Claims")]
        [GuidCheckFilter(new string[] { "rid" })]
        public async Task<ApiResponse> AssignClaim(string rid, [FromBody] ClaimDTO claim)
        {
            var role = await _roleManager.FindByIdAsync(rid);
            if (role == null)
                throw new ApiProblemDetailsException($"Record with id: {rid} does not exist.", 404);

            //Assign claim to role
            IList<ClaimDTO> claims = _mapper.Map<List<ClaimDTO>>(await _roleManager.GetClaimsAsync(role));

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
            claims = _mapper.Map<List<ClaimDTO>>(await _roleManager.GetClaimsAsync(role));

            var roleWithClaimsDto = _mapper.Map<IdentityRoleWithClaimsDTO>(role);
            roleWithClaimsDto.Claims = claims;

            return new ApiResponse(roleWithClaimsDto, 200);
        }

        [HttpDelete]
        [Authorize(Roles.WithdrawPermission)]
        [Route("/api/Roles/{rid}/Claims/{claimValue}")]
        [GuidCheckFilter(new string[] { "rid" })]
        public async Task<IActionResult> WithdrawClaim(string rid, string claimValue)
        {
            var role = await _roleManager.FindByIdAsync(rid);
            if (role == null)
                throw new ApiProblemDetailsException($"Record with id: {rid} does not exist.", 404);

            //Assign claim from role
            IList<Claim> claims = await _roleManager.GetClaimsAsync(role);

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
