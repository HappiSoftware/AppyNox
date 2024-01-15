using AppyNox.Services.Authentication.Application.Dtos.IdentityRoleDtos.Models.Base;
using AppyNox.Services.Authentication.Application.Dtos.IdentityUserDtos.Models.Base;
using AppyNox.Services.Authentication.Application.Dtos.IdentityUserDtos.Models.Extended;
using AppyNox.Services.Authentication.SharedEvents.Events;
using AppyNox.Services.Authentication.WebAPI.ControllerDependencies;
using AppyNox.Services.Authentication.WebAPI.ExceptionExtensions.Base;
using AppyNox.Services.Authentication.WebAPI.Filters;
using AppyNox.Services.Authentication.WebAPI.Utilities;
using AppyNox.Services.Base.Application.ExceptionExtensions;
using AppyNox.Services.Base.Core.AsyncLocals;
using Asp.Versioning;
using AutoWrapper.Wrappers;
using FluentValidation.Results;
using MassTransit;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net;
using ValidationResult = FluentValidation.Results.ValidationResult;

namespace AppyNox.Services.Authentication.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiVersion("1.0")]
    [ApiController]
    [JwtTokenValidate]
    public class UsersController : ControllerBase
    {
        #region [ Fields ]

        private readonly UsersControllerBaseDependencies _baseDependencies;

        private readonly IPublishEndpoint _publishEndpoint;

        #endregion

        #region [ Public Constructors ]

        public UsersController(UsersControllerBaseDependencies usersControllerBaseDependencies, IPublishEndpoint publishEndpoint)
        {
            _baseDependencies = usersControllerBaseDependencies;
            _publishEndpoint = publishEndpoint;
        }

        #endregion

        #region [ CRUD Operations ]

        [HttpGet]
        [Authorize(Permissions.Users.View)]
        public async Task<ApiResponse> GetAll()
        {
            var entities = await _baseDependencies.UserManager.Users.ToListAsync();
            object response = new
            {
                count = _baseDependencies.UserManager.Users.Count().ToString(),
                roles = _baseDependencies.Mapper.Map(entities, entities.GetType(), typeof(List<IdentityUserDto>))
            };

            return new ApiResponse(response);
        }

        [HttpGet("{id}")]
        [Authorize(Permissions.Users.View)]
        public async Task<ApiResponse> GetById(Guid id)
        {
            var identityUser = await _baseDependencies.UserManager.FindByIdAsync(id.ToString());

            if (identityUser == null)
            {
                throw new AuthenticationServiceException("User Not Found", (int)HttpStatusCode.NotFound);
            }

            return new ApiResponse(_baseDependencies.Mapper.Map(identityUser, identityUser.GetType(), typeof(IdentityUserDto)));
        }

        [HttpPut("{id}")]
        [Authorize(Permissions.Users.Edit)]
        public async Task<IActionResult> Put(Guid id, IdentityUserUpdateDto identityUserUpdateDto)
        {
            if (id.ToString() != identityUserUpdateDto.Id)
            {
                throw new AuthenticationServiceException("Ids don't match", (int)HttpStatusCode.UnprocessableEntity);
            }

            var existingUser = await _baseDependencies.UserManager.FindByIdAsync(id.ToString());
            if (existingUser == null)
            {
                throw new AuthenticationServiceException("User Not Found", (int)HttpStatusCode.NotFound);
            }

            var concurrencyStamp = existingUser.ConcurrencyStamp;
            existingUser.UserName = identityUserUpdateDto.UserName;

            var result = await _baseDependencies.UserValidator.ValidateAsync(_baseDependencies.UserManager, existingUser);
            if (!result.Succeeded)
            {
                ValidationResult validationResult = new();
                foreach (var error in result.Errors)
                {
                    ValidationFailure validationFailure = new(error.Code, error.Description);
                    validationResult.Errors.Add(validationFailure);
                }
                throw new FluentValidationException(typeof(IdentityUser), validationResult);
            }

            try
            {
                existingUser.ConcurrencyStamp = concurrencyStamp;
                IdentityResult identityResuls = await _baseDependencies.UserManager.UpdateAsync(existingUser);
                if (!identityResuls.Succeeded)
                {
                    ValidationResult validationResult = new();
                    foreach (var error in identityResuls.Errors)
                    {
                        ValidationFailure validationFailure = new(error.Code, error.Description);
                        validationResult.Errors.Add(validationFailure);
                    }
                    throw new FluentValidationException(typeof(IdentityUser), validationResult);
                }
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await IdentityUserExists(id))
                {
                    throw new AuthenticationServiceException("User Not Found", (int)HttpStatusCode.NotFound);
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        [HttpPost]
        [Authorize(Permissions.Users.Create)]
        public async Task<IActionResult> Post(IdentityUserCreateDto registerDto)
        {
            StartUserCreationMessage startUserCreationEvent = new
            (
                CorrelationContext.CorrelationId,
                registerDto.LicenseKey,
                registerDto.UserName,
                registerDto.Email,
                registerDto.Password,
                registerDto.ConfirmPassword
            );
            await _publishEndpoint.Publish(startUserCreationEvent);

            return Accepted();
        }

        [HttpDelete("{id}")]
        [Authorize(Permissions.Users.Delete)]
        public async Task<IActionResult> Delete(Guid id)
        {
            await _publishEndpoint.Publish(new DeleteApplicationUserMessage(CorrelationContext.CorrelationId, id));
            return Accepted();
        }

        #endregion

        #region [ Private Methods ]

        private async Task<bool> IdentityUserExists(Guid id)
        {
            return await _baseDependencies.UserManager.Users.AnyAsync(u => id.ToString() == u.Id);
        }

        #endregion

        #region [ User-Roles Operations ]

        [HttpGet]
        [Authorize(Permissions.Users.View)]
        [Route("/api/Users/{uid}/Roles")]
        public async Task<ApiResponse> GetRoles(Guid uid)
        {
            var user = await _baseDependencies.UserManager.FindByIdAsync(uid.ToString());
            if (user == null)
                throw new AuthenticationServiceException($"Record with id: {uid} does not exist.", (int)HttpStatusCode.NotFound);
            IList<string> roleNames = await _baseDependencies.UserManager.GetRolesAsync(user);
            List<IdentityRoleDto> roles = new List<IdentityRoleDto>();

            foreach (var item in roleNames)
            {
                roles.Add(_baseDependencies.Mapper.Map<IdentityRoleDto>(await _baseDependencies.RoleManager.FindByNameAsync(item)));
            }

            var userWithRolesDto = _baseDependencies.Mapper.Map<IdentityUserWithRolesDto>(user);
            userWithRolesDto.Roles = roles;
            return new ApiResponse(userWithRolesDto);
        }

        [HttpPost]
        [Authorize(Permissions.Users.Edit)]
        [Route("/api/Users/{uid}/Roles/{rid}")]
        public async Task<ApiResponse> AssignRole(Guid uid, Guid rid)
        {
            var user = await _baseDependencies.UserManager.FindByIdAsync(uid.ToString());
            var role = await _baseDependencies.RoleManager.FindByIdAsync(rid.ToString());
            if (user == null)
                throw new AuthenticationServiceException($"Record with id: {uid} does not exist.", (int)HttpStatusCode.NotFound);
            if (role == null || string.IsNullOrEmpty(role.Name))
                throw new AuthenticationServiceException($"Record with id: {rid} does not exist.", (int)HttpStatusCode.NotFound);

            //Assign the role from user
            IdentityResult response = await _baseDependencies.UserManager.AddToRoleAsync(user, role.Name);
            if (!response.Succeeded)
            {
                ValidationResult validationResult = new();
                foreach (var error in response.Errors)
                {
                    ValidationFailure validationFailure = new(error.Code, error.Description);
                    validationResult.Errors.Add(validationFailure);
                }
                throw new FluentValidationException(typeof(IdentityUser), validationResult);
            }
            IList<string> roleNames = await _baseDependencies.UserManager.GetRolesAsync(user);

            List<IdentityRoleDto> roles = new List<IdentityRoleDto>();

            foreach (var item in roleNames)
            {
                roles.Add(_baseDependencies.Mapper.Map<IdentityRoleDto>(await _baseDependencies.RoleManager.FindByNameAsync(item)));
            }

            var userWithRolesDto = _baseDependencies.Mapper.Map<IdentityUserWithRolesDto>(user);
            userWithRolesDto.Roles = roles;

            return new ApiResponse("Role assigned to user successfully.", userWithRolesDto);
        }

        [HttpDelete]
        [Authorize(Permissions.Users.Edit)]
        [Route("/api/Users/{uid}/Roles/{rid}")]
        public async Task<IActionResult> WithdrawRole(Guid uid, Guid rid)
        {
            var user = await _baseDependencies.UserManager.FindByIdAsync(uid.ToString());
            var role = await _baseDependencies.RoleManager.FindByIdAsync(rid.ToString());
            if (user == null)
                throw new AuthenticationServiceException($"Record with id: {uid} does not exist.", (int)HttpStatusCode.NotFound);
            if (role == null || string.IsNullOrEmpty(role.Name))
                throw new AuthenticationServiceException($"Record with id: {rid} does not exist.", (int)HttpStatusCode.NotFound);

            //Withdraw the role from user
            IdentityResult response = await _baseDependencies.UserManager.RemoveFromRoleAsync(user, role.Name);

            if (!response.Succeeded)
            {
                ValidationResult validationResult = new();
                foreach (var error in response.Errors)
                {
                    ValidationFailure validationFailure = new(error.Code, error.Description);
                    validationResult.Errors.Add(validationFailure);
                }
                throw new FluentValidationException(typeof(IdentityUser), validationResult);
            }

            return NoContent();
        }

        #endregion
    }
}