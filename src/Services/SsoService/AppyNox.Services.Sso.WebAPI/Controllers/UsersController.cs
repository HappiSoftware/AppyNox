using AppyNox.Services.Base.Application.Exceptions;
using AppyNox.Services.Base.Core.AsyncLocals;
using AppyNox.Services.Base.Core.Extensions;
using AppyNox.Services.Sso.Application.DTOs.ApplicationRoleDTOs.Models;
using AppyNox.Services.Sso.Application.DTOs.ApplicationUserDTOs.Models;
using AppyNox.Services.Sso.Application.Permission;
using AppyNox.Services.Sso.SharedEvents.Events;
using AppyNox.Services.Sso.WebAPI.ControllerDependencies;
using AppyNox.Services.Sso.WebAPI.Exceptions.Base;
using AppyNox.Services.Sso.WebAPI.Extensions;
using AppyNox.Services.Sso.WebAPI.Localization;
using Asp.Versioning;
using FluentValidation;
using FluentValidation.Results;
using MassTransit;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net;
using ValidationResult = FluentValidation.Results.ValidationResult;

namespace AppyNox.Services.Sso.WebAPI.Controllers;

[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/[controller]")]
public class UsersController(UsersControllerBaseDependencies usersControllerBaseDependencies, IPublishEndpoint publishEndpoint, IValidator<ApplicationUserCreateDto> userDtoValidator) : ControllerBase
{
    #region [ Fields ]

    private readonly UsersControllerBaseDependencies _baseDependencies = usersControllerBaseDependencies;

    private readonly IPublishEndpoint _publishEndpoint = publishEndpoint;

    private readonly IValidator<ApplicationUserCreateDto> _userDtoValidator = userDtoValidator;

    #endregion

    #region [ CRUD Operations ]

    [HttpGet]
    [Authorize(Permissions.Users.View)]
    public async Task<IActionResult> GetAll()
    {
        var entities = await _baseDependencies.UserManager.Users.ToListAsync();
        object response = new
        {
            count = _baseDependencies.UserManager.Users.Count().ToString(),
            roles = _baseDependencies.Mapper.Map(entities, entities.GetType(), typeof(List<ApplicationUserDto>))
        };

        return Ok(response);
    }

    [HttpGet("{id}")]
    [Authorize(Permissions.Users.View)]
    public async Task<IActionResult> GetById(Guid id)
    {
        var identityUser = await _baseDependencies.UserManager.FindByIdAsync(id.ToString());

        if (identityUser == null)
        {
            throw new NoxSsoApiException(NoxSsoApiResourceService.UserNotFound, statusCode: (int)HttpStatusCode.NotFound);
        }

        return Ok(_baseDependencies.Mapper.Map(identityUser, identityUser.GetType(), typeof(ApplicationUserDto)));
    }

    [HttpPut("{id}")]
    [Authorize(Permissions.Users.Edit)]
    public async Task<IActionResult> Put(Guid id, ApplicationUserUpdateDto identityUserUpdateDto)
    {
        if (id != identityUserUpdateDto.Id)
        {
            throw new NoxSsoApiException(NoxSsoApiResourceService.IdMismatch, statusCode: (int)HttpStatusCode.UnprocessableEntity);
        }

        var existingUser = await _baseDependencies.UserManager.FindByIdAsync(id.ToString())
            ?? throw new NoxSsoApiException(NoxSsoApiResourceService.UserNotFound, statusCode: (int)HttpStatusCode.NotFound);

        var concurrencyStamp = existingUser.ConcurrencyStamp;
        existingUser.UserName = identityUserUpdateDto.UserName;

        var result = await _baseDependencies.UserValidator.ValidateAsync(_baseDependencies.UserManager, existingUser);
        result.HandleValidationResult();

        try
        {
            existingUser.ConcurrencyStamp = concurrencyStamp;
            IdentityResult identityResults = await _baseDependencies.UserManager.UpdateAsync(existingUser);
            identityResults.HandleValidationResult();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!await IdentityUserExists(id))
            {
                throw new NoxSsoApiException(NoxSsoApiResourceService.UserNotFound, statusCode: (int)HttpStatusCode.NotFound);
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
    public async Task<IActionResult> Post(ApplicationUserCreateDto registerDto)
    {
        ValidationResult validationResult = await _userDtoValidator.ValidateAsync(registerDto);
        if(!validationResult.IsValid)
        {
            throw new NoxFluentValidationException(typeof(ApplicationUserCreateDto), validationResult);
        }
        StartUserCreationMessage startUserCreationEvent = new
        (
            NoxContext.CorrelationId,
            registerDto.LicenseKey,
            registerDto.UserName,
            registerDto.Email,
            registerDto.Password,
            registerDto.ConfirmPassword,
            registerDto.Name,
            registerDto.Surname,
            registerDto.Code
        );
        await _publishEndpoint.Publish(startUserCreationEvent);

        return Accepted();
    }

    [HttpDelete("{id}")]
    [Authorize(Permissions.Users.Delete)]
    public async Task<IActionResult> Delete(Guid id)
    {
        await _publishEndpoint.Publish(new DeleteApplicationUserMessage(NoxContext.CorrelationId, id));
        return Accepted();
    }

    #endregion

    #region [ Private Methods ]

    private async Task<bool> IdentityUserExists(Guid id)
    {
        return await _baseDependencies.UserManager.Users.AnyAsync(u => id == u.Id);
    }

    #endregion

    #region [ User-Roles Operations ]

    [HttpGet]
    [Authorize(Permissions.Users.View)]
    [Route("/api/Users/{uid}/Roles")]
    public async Task<IActionResult> GetRoles(Guid uid)
    {
        var user = await _baseDependencies.UserManager.FindByIdAsync(uid.ToString())
            ?? throw new NoxSsoApiException(NoxSsoApiResourceService.RecordNotFound.Format(uid), statusCode: (int)HttpStatusCode.NotFound);

        IList<string> roleNames = await _baseDependencies.UserManager.GetRolesAsync(user);
        List<ApplicationRoleDto> roles = [];

        foreach (var item in roleNames)
        {
            roles.Add(_baseDependencies.Mapper.Map<ApplicationRoleDto>(await _baseDependencies.RoleManager.FindByNameAsync(item)));
        }

        var userWithRolesDto = _baseDependencies.Mapper.Map<ApplicationUserWithRolesDto>(user);
        userWithRolesDto.Roles = roles;
        return Ok(userWithRolesDto);
    }

    [HttpPost]
    [Authorize(Permissions.Users.Edit)]
    [Route("/api/Users/{uid}/Roles/{rid}")]
    public async Task<IActionResult> AssignRole(Guid uid, Guid rid)
    {
        var user = await _baseDependencies.UserManager.FindByIdAsync(uid.ToString());
        var role = await _baseDependencies.RoleManager.FindByIdAsync(rid.ToString());
        if (user == null)
            throw new NoxSsoApiException(NoxSsoApiResourceService.RecordNotFound.Format(uid), statusCode: (int)HttpStatusCode.NotFound);
        if (role == null || string.IsNullOrEmpty(role.Name))
            throw new NoxSsoApiException(NoxSsoApiResourceService.RecordNotFound.Format(rid), statusCode: (int)HttpStatusCode.NotFound);

        //Assign the role from user
        IdentityResult response = await _baseDependencies.UserManager.AddToRoleAsync(user, role.Name);
        response.HandleValidationResult();

        IList<string> roleNames = await _baseDependencies.UserManager.GetRolesAsync(user);

        List<ApplicationRoleDto> roles = [];

        foreach (var item in roleNames)
        {
            roles.Add(_baseDependencies.Mapper.Map<ApplicationRoleDto>(await _baseDependencies.RoleManager.FindByNameAsync(item)));
        }

        var userWithRolesDto = _baseDependencies.Mapper.Map<ApplicationUserWithRolesDto>(user);
        userWithRolesDto.Roles = roles;
        return Ok(userWithRolesDto);
    }

    [HttpDelete]
    [Authorize(Permissions.Users.Edit)]
    [Route("/api/Users/{uid}/Roles/{rid}")]
    public async Task<IActionResult> WithdrawRole(Guid uid, Guid rid)
    {
        var user = await _baseDependencies.UserManager.FindByIdAsync(uid.ToString());
        var role = await _baseDependencies.RoleManager.FindByIdAsync(rid.ToString());
        if (user == null)
            throw new NoxSsoApiException(NoxSsoApiResourceService.RecordNotFound.Format(uid), statusCode: (int)HttpStatusCode.NotFound);
        if (role == null || string.IsNullOrEmpty(role.Name))
            throw new NoxSsoApiException(NoxSsoApiResourceService.RecordNotFound.Format(rid), statusCode: (int)HttpStatusCode.NotFound);

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
            throw new NoxFluentValidationException(typeof(IdentityUser), validationResult);
        }

        return NoContent();
    }

    #endregion
}