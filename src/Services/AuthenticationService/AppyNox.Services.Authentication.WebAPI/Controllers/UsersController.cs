using AppyNox.Services.Authentication.Application.Dtos.IdentityRoleDtos.Models.Base;
using AppyNox.Services.Authentication.Application.Dtos.IdentityUserDtos.Models.Base;
using AppyNox.Services.Authentication.Application.Dtos.IdentityUserDtos.Models.Extended;
using AppyNox.Services.Authentication.Application.Validators.IdentityUser;
using AppyNox.Services.Authentication.WebAPI.ControllerDependencies;
using AppyNox.Services.Authentication.WebAPI.Filters;
using AppyNox.Services.Authentication.WebAPI.Utilities;
using AppyNox.Services.Base.API.ViewModels;
using Asp.Versioning;
using AutoWrapper.Wrappers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AppyNox.Services.Authentication.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiVersion("1.0")]
    [ApiController]
    [JwtTokenValidate]
    public class UsersController : ControllerBase
    {
        #region [ Fields ]

        private readonly IdentityUserCreateDtoValidator _identityUserCreateDtoValidator;

        private readonly UsersControllerBaseDependencies _baseDependencies;

        #endregion

        #region [ Public Constructors ]

        public UsersController(UsersControllerBaseDependencies usersControllerBaseDependencies,
            IdentityUserCreateDtoValidator identityUserCreateDtoValidator)
        {
            _baseDependencies = usersControllerBaseDependencies;
            _identityUserCreateDtoValidator = identityUserCreateDtoValidator;
        }

        #endregion

        #region [ CRUD Operations ]

        [HttpGet]
        [Authorize(Permissions.Users.View)]
        public async Task<ApiResponse> GetAll([FromQuery] QueryParametersViewModel queryParameters)
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
        [GuidCheckFilter]
        public async Task<ApiResponse> GetById(string id, [FromQuery] QueryParametersViewModel queryParameters)
        {
            var identityUser = await _baseDependencies.UserManager.FindByIdAsync(id);

            if (identityUser == null)
            {
                throw new ApiProblemDetailsException("Not Found", 404);
            }

            return new ApiResponse(_baseDependencies.Mapper.Map(identityUser, identityUser.GetType(), typeof(IdentityUserDto)));
        }

        [HttpPut("{id}")]
        [Authorize(Permissions.Users.Edit)]
        [GuidCheckFilter]
        public async Task<IActionResult> Put(string id, IdentityUserUpdateDto identityUserUpdateDto)
        {
            if (id != identityUserUpdateDto.Id)
            {
                throw new ApiProblemDetailsException("Ids don't match", 422);
            }

            var existingUser = await _baseDependencies.UserManager.FindByIdAsync(id);
            if (existingUser == null)
            {
                throw new ApiProblemDetailsException("Role Not Found", 404);
            }

            var concurrencyStamp = existingUser.ConcurrencyStamp;
            existingUser.UserName = identityUserUpdateDto.UserName;

            var result = await _baseDependencies.UserValidator.ValidateAsync(_baseDependencies.UserManager, existingUser);
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
                existingUser.ConcurrencyStamp = concurrencyStamp;
                IdentityResult identityResuls = await _baseDependencies.UserManager.UpdateAsync(existingUser);
                if (!identityResuls.Succeeded)
                {
                    foreach (var item in identityResuls.Errors)
                    {
                        ModelState.AddModelError(item.Code, item.Description);
                    }
                    throw new ApiProblemDetailsException(ModelState);
                }
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await IdentityUserExists(id))
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
        [Authorize(Permissions.Users.Create)]
        public async Task<ApiResponse> Post(IdentityUserCreateDto registerDto)
        {
            var dtoValidationResult = await _identityUserCreateDtoValidator.ValidateAsync(registerDto);
            if (!dtoValidationResult.IsValid)
            {
                foreach (var error in dtoValidationResult.Errors)
                {
                    ModelState.AddModelError(error.ErrorCode, error.ErrorMessage);
                }
                throw new ApiProblemDetailsException(ModelState);
            }

            var userEntity = _baseDependencies.Mapper.Map<IdentityUser>(registerDto);
            var result = await _baseDependencies.UserValidator.ValidateAsync(_baseDependencies.UserManager, userEntity);
            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(error.Code, error.Description);
                }
                throw new ApiProblemDetailsException(ModelState);
            }

            var passwordResult = await _baseDependencies.PasswordValidator.ValidateAsync(_baseDependencies.UserManager, userEntity, registerDto.Password);

            if (!passwordResult.Succeeded)
            {
                foreach (var error in passwordResult.Errors)
                {
                    ModelState.AddModelError(error.Code, error.Description);
                }
                throw new ApiProblemDetailsException(ModelState);
            }

            userEntity.PasswordHash = _baseDependencies.PasswordHasher.HashPassword(userEntity, registerDto.Password);
            await _baseDependencies.UserManager.CreateAsync(userEntity);

            object response = new
            {
                id = userEntity.Id,
                value = registerDto
            };

            return new ApiResponse("New record has been created in the database.", response, 201);
        }

        [HttpDelete("{id}")]
        [Authorize(Permissions.Users.Delete)]
        [GuidCheckFilter]
        public async Task<IActionResult> Delete(string id)
        {
            var identityUser = await _baseDependencies.UserManager.FindByIdAsync(id);
            if (identityUser == null)
            {
                throw new ApiProblemDetailsException("Not Found", 404);
            }

            await _baseDependencies.UserManager.DeleteAsync(identityUser);

            return NoContent();
        }

        #endregion

        #region [ Private Methods ]

        private async Task<bool> IdentityUserExists(string id)
        {
            return await _baseDependencies.UserManager.Users.AnyAsync(u => id == u.Id);
        }

        #endregion

        #region [ User-Roles Operations ]

        [HttpGet]
        [Authorize(Permissions.Users.View)]
        [Route("/api/Users/{uid}/Roles")]
        [GuidCheckFilter("uid")]
        public async Task<ApiResponse> GetRoles(string uid)
        {
            var user = await _baseDependencies.UserManager.FindByIdAsync(uid);
            if (user == null)
                throw new ApiProblemDetailsException($"Record with id: {uid} does not exist.", 404);
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
        [GuidCheckFilter("uid,rid")]
        public async Task<ApiResponse> AssignRole(string uid, string rid)
        {
            var user = await _baseDependencies.UserManager.FindByIdAsync(uid);
            var role = await _baseDependencies.RoleManager.FindByIdAsync(rid);
            if (user == null)
                throw new ApiProblemDetailsException($"Record with id: {uid} does not exist.", 404);
            if (role == null || string.IsNullOrEmpty(role.Name))
                throw new ApiProblemDetailsException($"Record with id: {rid} does not exist.", 404);

            //Assign the role from user
            IdentityResult response = await _baseDependencies.UserManager.AddToRoleAsync(user, role.Name);
            if (!response.Succeeded)
            {
                foreach (var error in response.Errors)
                {
                    ModelState.AddModelError(error.Code, error.Description);
                }
                throw new ApiProblemDetailsException(ModelState);
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
        [GuidCheckFilter("uid,rid")]
        public async Task<IActionResult> WithdrawRole(string uid, string rid)
        {
            var user = await _baseDependencies.UserManager.FindByIdAsync(uid);
            var role = await _baseDependencies.RoleManager.FindByIdAsync(rid);
            if (user == null)
                throw new ApiProblemDetailsException($"Record with id: {uid} does not exist.", 404);
            if (role == null || string.IsNullOrEmpty(role.Name))
                throw new ApiProblemDetailsException($"Record with id: {rid} does not exist.", 404);

            //Withdraw the role from user
            IdentityResult response = await _baseDependencies.UserManager.RemoveFromRoleAsync(user, role.Name);

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
    }
}