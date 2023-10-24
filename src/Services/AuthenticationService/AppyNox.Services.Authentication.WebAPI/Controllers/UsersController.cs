using AppyNox.Services.Authentication.Application.DTOs.IdentityRoleDTOs.Models;
using AppyNox.Services.Authentication.Application.DTOs.IdentityUserDTOs.Models;
using AppyNox.Services.Authentication.Application.Validators;
using AppyNox.Services.Authentication.WebAPI.Filters;
using AppyNox.Services.Authentication.WebAPI.Helpers;
using AppyNox.Services.Authentication.WebAPI.Utilities;
using Asp.Versioning;
using AutoMapper;
using AutoWrapper.Wrappers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security;

namespace AppyNox.Services.Authentication.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiVersion("1.0")]
    [ApiController]
    [JwtTokenValidate]
    public class UsersController : ControllerBase
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IUserValidator<IdentityUser> _userValidator;
        private readonly IMapper _mapper;
        private readonly DtoMappingHelper<IdentityUser> _dtoMappingHelper;
        private readonly PasswordValidator<IdentityUser> _passwordValidator;
        private readonly PasswordHasher<IdentityUser> _passwordHasher;
        private readonly IdentityUserCreateDTOValidator _identityUserCreateDTOValidator;

        public UsersController(IMapper mapper, UserManager<IdentityUser> userManager,
            DtoMappingHelper<IdentityUser> dtoMappingHelper,
            IUserValidator<IdentityUser> userValidator, RoleManager<IdentityRole> roleManager, PasswordValidator<IdentityUser> passwordValidator,
            PasswordHasher<IdentityUser> passwordHasher,
            IdentityUserCreateDTOValidator identityUserCreateDTOValidator)
        {
            _mapper = mapper;
            _userManager = userManager;
            _dtoMappingHelper = dtoMappingHelper;
            _userValidator = userValidator;
            _roleManager = roleManager;
            _passwordValidator = passwordValidator;
            _passwordHasher = passwordHasher;
            _identityUserCreateDTOValidator = identityUserCreateDTOValidator;
        }

        #region [ CRUD Operations ]

        [HttpGet]
        [Authorize(Permissions.Users.View)]
        public async Task<ApiResponse> GetAll([FromQuery] string? detailLevel)
        {
            var entities = await _userManager.Users.ToListAsync();
            object response = new
            {
                count = _userManager.Users.Count().ToString(),
                roles = _mapper.Map(entities, entities.GetType(), typeof(IEnumerable<>).MakeGenericType(_dtoMappingHelper.GetLeveledDtoType(detailLevel)))
            };

            return new ApiResponse(response);
        }

        [HttpGet("{id}")]
        [Authorize(Permissions.Users.View)]
        [GuidCheckFilter]
        public async Task<ApiResponse> GetById(string id, [FromQuery] string? detailLevel)
        {
            var identityUser = await _userManager.FindByIdAsync(id);

            if (identityUser == null)
            {
                throw new ApiProblemDetailsException("Not Found", 404);
            }

            return new ApiResponse(_mapper.Map(identityUser, identityUser.GetType(), _dtoMappingHelper.GetLeveledDtoType(detailLevel)));
        }

        [HttpPut("{id}")]
        [Authorize(Permissions.Users.Edit)]
        [GuidCheckFilter]
        public async Task<IActionResult> Put(string id, IdentityUserUpdateDTO identityUserUpdateDTO)
        {
            if (id != identityUserUpdateDTO.Id)
            {
                throw new ApiProblemDetailsException("Ids don't match", 422);
            }

            var existingUser = await _userManager.FindByIdAsync(id);
            if (existingUser == null)
            {
                throw new ApiProblemDetailsException("Role Not Found", 404);
            }

            var concurrencyStamp = existingUser.ConcurrencyStamp;
            existingUser.UserName = identityUserUpdateDTO.UserName;

            var result = await _userValidator.ValidateAsync(_userManager, existingUser);
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
                IdentityResult identityResuls = await _userManager.UpdateAsync(existingUser);
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
        public async Task<ApiResponse> Post(IdentityUserCreateDTO registerDTO)
        {
            var dtoValidationResult = await _identityUserCreateDTOValidator.ValidateAsync(registerDTO);
            if (!dtoValidationResult.IsValid)
            {
                foreach (var error in dtoValidationResult.Errors)
                {
                    ModelState.AddModelError(error.ErrorCode, error.ErrorMessage);
                }
                throw new ApiProblemDetailsException(ModelState);
            }

            var userEntity = _mapper.Map<IdentityUser>(registerDTO);
            var result = await _userValidator.ValidateAsync(_userManager, userEntity);
            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(error.Code, error.Description);
                }
                throw new ApiProblemDetailsException(ModelState);
            }

            var passwordResult = await _passwordValidator.ValidateAsync(_userManager, userEntity, registerDTO.Password);

            if (!passwordResult.Succeeded)
            {
                foreach (var error in passwordResult.Errors)
                {
                    ModelState.AddModelError(error.Code, error.Description);
                }
                throw new ApiProblemDetailsException(ModelState);
            }

            userEntity.PasswordHash = _passwordHasher.HashPassword(userEntity, registerDTO.Password);
            await _userManager.CreateAsync(userEntity);

            object response = new
            {
                id = userEntity.Id,
                value = registerDTO
            };

            return new ApiResponse("New record has been created in the database.", response, 201);
        }

        [HttpDelete("{id}")]
        [Authorize(Permissions.Users.Delete)]
        [GuidCheckFilter]
        public async Task<IActionResult> Delete(string id)
        {
            var identityUser = await _userManager.FindByIdAsync(id);
            if (identityUser == null)
            {
                throw new ApiProblemDetailsException("Not Found", 404);
            }

            await _userManager.DeleteAsync(identityUser);

            return NoContent();
        }

        #endregion

        #region [ Private Methods ]

        private async Task<bool> IdentityUserExists(string id)
        {
            return await _userManager.Users.AnyAsync(u => id == u.Id);
        }

        #endregion

        #region [ User-Roles Operations ]

        [HttpGet]
        [Authorize(Permissions.Users.View)]
        [Route("/api/Users/{uid}/Roles")]
        [GuidCheckFilter(new string[] { "uid" })]
        public async Task<ApiResponse> GetRoles(string uid)
        {
            var user = await _userManager.FindByIdAsync(uid);
            if (user == null)
                throw new ApiProblemDetailsException($"Record with id: {uid} does not exist.", 404);
            IList<string> roleNames = await _userManager.GetRolesAsync(user);
            IList<IdentityRoleDTO> roles = new List<IdentityRoleDTO>();

            foreach (var item in roleNames)
            {
                roles.Add(_mapper.Map<IdentityRoleDTO>(await _roleManager.FindByNameAsync(item)));
            }

            var userWithRolesDTO = _mapper.Map<IdentityUserWithRolesDTO>(user);
            userWithRolesDTO.Roles = roles;
            return new ApiResponse(userWithRolesDTO);
        }

        [HttpPost]
        [Authorize(Permissions.Users.Edit)]
        [Route("/api/Users/{uid}/Roles/{rid}")]
        [GuidCheckFilter(new string[] { "uid", "rid" })]
        public async Task<ApiResponse> AssignRole(string uid, string rid)
        {
            var user = await _userManager.FindByIdAsync(uid);
            var role = await _roleManager.FindByIdAsync(rid);
            if (user == null)
                throw new ApiProblemDetailsException($"Record with id: {uid} does not exist.", 404);
            if (role == null || string.IsNullOrEmpty(role.Name))
                throw new ApiProblemDetailsException($"Record with id: {rid} does not exist.", 404);

            //Assign the role from user
            IdentityResult response = await _userManager.AddToRoleAsync(user, role.Name);
            if (!response.Succeeded)
            {
                foreach (var error in response.Errors)
                {
                    ModelState.AddModelError(error.Code, error.Description);
                }
                throw new ApiProblemDetailsException(ModelState);
            }
            IList<string> roleNames = await _userManager.GetRolesAsync(user);

            IList<IdentityRoleDTO> roles = new List<IdentityRoleDTO>();

            foreach (var item in roleNames)
            {
                roles.Add(_mapper.Map<IdentityRoleDTO>(await _roleManager.FindByNameAsync(item)));
            }

            var userWithRolesDTO = _mapper.Map<IdentityUserWithRolesDTO>(user);
            userWithRolesDTO.Roles = roles;

            return new ApiResponse("Role assigned to user successfully.", userWithRolesDTO);
        }

        [HttpDelete]
        [Authorize(Permissions.Users.Edit)]
        [Route("/api/Users/{uid}/Roles/{rid}")]
        [GuidCheckFilter(new string[] { "uid", "rid" })]
        public async Task<IActionResult> WithdrawRole(string uid, string rid)
        {
            var user = await _userManager.FindByIdAsync(uid);
            var role = await _roleManager.FindByIdAsync(rid);
            if (user == null)
                throw new ApiProblemDetailsException($"Record with id: {uid} does not exist.", 404);
            if (role == null || string.IsNullOrEmpty(role.Name))
                throw new ApiProblemDetailsException($"Record with id: {rid} does not exist.", 404);

            //Withdraw the role from user
            IdentityResult response = await _userManager.RemoveFromRoleAsync(user, role.Name);

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
