using AppyNox.Services.Authentication.Application.Dtos.IdentityUserDtos.Models.Base;
using AppyNox.Services.Authentication.Application.MediatR.Commands;
using AppyNox.Services.Authentication.Application.Validators.IdentityUser;
using AppyNox.Services.Authentication.Domain.Entities;
using AppyNox.Services.Base.Application.ExceptionExtensions;
using AutoMapper;
using FluentValidation.Results;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace AppyNox.Services.Authentication.Application.MediatR.Handlers
{
    internal sealed class CreateUserCommandHandler(IdentityUserCreateDtoValidator identityUserCreateDtoValidator, IMapper mapper,
        UserManager<ApplicationUser> userManager, IUserValidator<ApplicationUser> userValidator, PasswordValidator<ApplicationUser> passwordValidator,
        PasswordHasher<ApplicationUser> passwordHasher)
        : IRequestHandler<CreateUserCommand, (Guid id, IdentityUserDto dto)>
    {
        #region Fields

        private readonly IdentityUserCreateDtoValidator _identityUserCreateDtoValidator = identityUserCreateDtoValidator;

        private readonly IMapper _mapper = mapper;

        private readonly UserManager<ApplicationUser> _userManager = userManager;

        private readonly IUserValidator<ApplicationUser> _userValidator = userValidator;

        private readonly PasswordValidator<ApplicationUser> _passwordValidator = passwordValidator;

        private readonly PasswordHasher<ApplicationUser> _passwordHasher = passwordHasher;

        #endregion

        #region Public Methods

        public async Task<(Guid id, IdentityUserDto dto)> Handle(CreateUserCommand request, CancellationToken cancellationToken)
        {
            var dtoValidationResult = await _identityUserCreateDtoValidator.ValidateAsync(request.IdentityUserCreateDto, cancellationToken);
            if (!dtoValidationResult.IsValid)
            {
                throw new FluentValidationException(typeof(ApplicationUser), dtoValidationResult);
            }

            var userEntity = _mapper.Map<ApplicationUser>(request.IdentityUserCreateDto);
            var result = await _userValidator.ValidateAsync(_userManager, userEntity);
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

            var passwordResult = await _passwordValidator.ValidateAsync(_userManager, userEntity, request.IdentityUserCreateDto.Password);

            if (!passwordResult.Succeeded)
            {
                ValidationResult validationResult = new();
                foreach (var error in passwordResult.Errors)
                {
                    ValidationFailure validationFailure = new(error.Code, error.Description);
                    validationResult.Errors.Add(validationFailure);
                }
                throw new FluentValidationException(typeof(ApplicationUser), validationResult);
            }

            userEntity.PasswordHash = _passwordHasher.HashPassword(userEntity, request.IdentityUserCreateDto.Password);
            await _userManager.CreateAsync(userEntity);

            return (Guid.Parse(userEntity.Id), _mapper.Map<IdentityUserDto>(userEntity));
        }

        #endregion
    }
}