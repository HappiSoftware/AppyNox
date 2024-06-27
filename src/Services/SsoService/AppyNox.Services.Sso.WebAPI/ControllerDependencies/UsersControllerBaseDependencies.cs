using AppyNox.Services.Sso.Domain.Entities;
using AutoMapper;
using Microsoft.AspNetCore.Identity;

namespace AppyNox.Services.Sso.WebAPI.ControllerDependencies
{
    /// <summary>
    /// Provides dependencies for user-related controllers.
    /// </summary>
    public class UsersControllerBaseDependencies(IMapper mapper, UserManager<ApplicationUser> userManager,
        IUserValidator<ApplicationUser> userValidator, RoleManager<ApplicationRole> roleManager, IPasswordValidator<ApplicationUser> passwordValidator,
        IPasswordHasher<ApplicationUser> passwordHasher)
    {
        #region [ Fields ]

        private readonly UserManager<ApplicationUser> _userManager = userManager;

        private readonly RoleManager<ApplicationRole> _roleManager = roleManager;

        private readonly IUserValidator<ApplicationUser> _userValidator = userValidator;

        private readonly IMapper _mapper = mapper;

        private readonly IPasswordValidator<ApplicationUser> _passwordValidator = passwordValidator;

        private readonly IPasswordHasher<ApplicationUser> _passwordHasher = passwordHasher;

        #endregion
        #region [ Public Constructors ]

        #endregion

        #region [ Properties ]

        public UserManager<ApplicationUser> UserManager => _userManager;

        public RoleManager<ApplicationRole> RoleManager => _roleManager;

        public IUserValidator<ApplicationUser> UserValidator => _userValidator;

        public IMapper Mapper => _mapper;

        public IPasswordValidator<ApplicationUser> PasswordValidator => _passwordValidator;

        public IPasswordHasher<ApplicationUser> PasswordHasher => _passwordHasher;

        #endregion
    }
}