using AppyNox.Services.Authentication.Domain.Entities;
using AutoMapper;
using Microsoft.AspNetCore.Identity;

namespace AppyNox.Services.Authentication.WebAPI.ControllerDependencies
{
    /// <summary>
    /// Provides dependencies for user-related controllers.
    /// </summary>
    public class UsersControllerBaseDependencies
    {
        #region [ Fields ]

        private readonly UserManager<ApplicationUser> _userManager;

        private readonly RoleManager<IdentityRole> _roleManager;

        private readonly IUserValidator<ApplicationUser> _userValidator;

        private readonly IMapper _mapper;

        private readonly PasswordValidator<ApplicationUser> _passwordValidator;

        private readonly PasswordHasher<ApplicationUser> _passwordHasher;

        #endregion

        #region [ Public Constructors ]

        public UsersControllerBaseDependencies(IMapper mapper, UserManager<ApplicationUser> userManager,
            IUserValidator<ApplicationUser> userValidator, RoleManager<IdentityRole> roleManager, PasswordValidator<ApplicationUser> passwordValidator,
            PasswordHasher<ApplicationUser> passwordHasher)
        {
            _mapper = mapper;
            _userManager = userManager;
            _userValidator = userValidator;
            _roleManager = roleManager;
            _passwordValidator = passwordValidator;
            _passwordHasher = passwordHasher;
        }

        #endregion

        #region [ Properties ]

        public UserManager<ApplicationUser> UserManager => _userManager;

        public RoleManager<IdentityRole> RoleManager => _roleManager;

        public IUserValidator<ApplicationUser> UserValidator => _userValidator;

        public IMapper Mapper => _mapper;

        public PasswordValidator<ApplicationUser> PasswordValidator => _passwordValidator;

        public PasswordHasher<ApplicationUser> PasswordHasher => _passwordHasher;

        #endregion
    }
}