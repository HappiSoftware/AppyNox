using AutoMapper;
using Microsoft.AspNetCore.Identity;

namespace AppyNox.Services.Authentication.WebAPI.ControllerDependencies
{
    public class UsersControllerBaseDependencies
    {
        #region [ Fields ]

        private readonly UserManager<IdentityUser> _userManager;

        private readonly RoleManager<IdentityRole> _roleManager;

        private readonly IUserValidator<IdentityUser> _userValidator;

        private readonly IMapper _mapper;

        private readonly PasswordValidator<IdentityUser> _passwordValidator;

        private readonly PasswordHasher<IdentityUser> _passwordHasher;

        #endregion

        #region [ Public Constructors ]

        public UsersControllerBaseDependencies(IMapper mapper, UserManager<IdentityUser> userManager,
            IUserValidator<IdentityUser> userValidator, RoleManager<IdentityRole> roleManager, PasswordValidator<IdentityUser> passwordValidator,
            PasswordHasher<IdentityUser> passwordHasher)
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

        public UserManager<IdentityUser> UserManager => _userManager;

        public RoleManager<IdentityRole> RoleManager => _roleManager;

        public IUserValidator<IdentityUser> UserValidator => _userValidator;

        public IMapper Mapper => _mapper;

        public PasswordValidator<IdentityUser> PasswordValidator => _passwordValidator;

        public PasswordHasher<IdentityUser> PasswordHasher => _passwordHasher;

        #endregion
    }
}