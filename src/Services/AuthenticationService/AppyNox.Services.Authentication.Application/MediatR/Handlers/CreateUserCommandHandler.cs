using AppyNox.Services.Authentication.Application.DTOs.ApplicationUserDTOs.Models;
using AppyNox.Services.Authentication.Application.ExceptionExtensions;
using AppyNox.Services.Authentication.Application.MediatR.Commands;
using AppyNox.Services.Authentication.Application.Validators.ApplicationUserValidators;
using AppyNox.Services.Authentication.Domain.Entities;
using AppyNox.Services.Base.Application.ExceptionExtensions;
using AutoMapper;
using FluentValidation.Results;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace AppyNox.Services.Authentication.Application.MediatR.Handlers
{
    internal sealed class CreateUserCommandHandler(ApplicationUserCreateDtoValidator identityUserCreateDtoValidator,
                                                   IMapper mapper,
                                                   UserManager<ApplicationUser> userManager,
                                                   IUserValidator<ApplicationUser> userValidator,
                                                   PasswordValidator<ApplicationUser> passwordValidator,
                                                   PasswordHasher<ApplicationUser> passwordHasher)
        : IRequestHandler<CreateUserCommand, (Guid id, ApplicationUserDto dto)>
    {
        #region [ Fields ]

        private readonly ApplicationUserCreateDtoValidator _identityUserCreateDtoValidator = identityUserCreateDtoValidator;

        private readonly IMapper _mapper = mapper;

        private readonly UserManager<ApplicationUser> _userManager = userManager;

        private readonly IUserValidator<ApplicationUser> _userValidator = userValidator;

        private readonly PasswordValidator<ApplicationUser> _passwordValidator = passwordValidator;

        private readonly PasswordHasher<ApplicationUser> _passwordHasher = passwordHasher;

        #endregion

        #region [ Public Methods ]

        public async Task<(Guid id, ApplicationUserDto dto)> Handle(CreateUserCommand request, CancellationToken cancellationToken)
        {
            try
            {
                ValidationResult dtoValidationResult = await _identityUserCreateDtoValidator.ValidateAsync(request.IdentityUserCreateDto, cancellationToken);
                if (!dtoValidationResult.IsValid)
                {
                    throw new FluentValidationException(typeof(ApplicationUser), dtoValidationResult);
                }

                ApplicationUser userEntity = _mapper.Map<ApplicationUser>(request.IdentityUserCreateDto);
                IdentityResult result = await _userValidator.ValidateAsync(_userManager, userEntity);
                if (!result.Succeeded)
                {
                    ValidationResult validationResult = new();
                    foreach (var error in result.Errors)
                    {
                        ValidationFailure validationFailure = new(error.Code, error.Description);
                        validationResult.Errors.Add(validationFailure);
                    }
                    throw new FluentValidationException(typeof(ApplicationUser), validationResult);
                }

                IdentityResult passwordResult = await _passwordValidator.ValidateAsync(_userManager, userEntity, request.IdentityUserCreateDto.Password);

                if (!passwordResult.Succeeded)
                {
                    ValidationResult validationResult = new();
                    foreach (IdentityError error in passwordResult.Errors)
                    {
                        ValidationFailure validationFailure = new(error.Code, error.Description);
                        validationResult.Errors.Add(validationFailure);
                    }
                    throw new FluentValidationException(typeof(ApplicationUser), validationResult);
                }

                userEntity.PasswordHash = _passwordHasher.HashPassword(userEntity, request.IdentityUserCreateDto.Password);
                await _userManager.CreateAsync(userEntity);

                return (userEntity.Id, _mapper.Map<ApplicationUserDto>(userEntity));
            }
            catch (Exception ex)
            {
                throw new NoxSsoApplicationException(ex, (int)NoxSsoApplicationExceptionCode.CreateUserCommandError);
            }
        }

        #endregion
    }
}