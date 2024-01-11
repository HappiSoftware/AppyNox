using AppyNox.Services.Authentication.Application.Validators.SharedRules;
using AppyNox.Services.Authentication.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace AppyNox.Services.Authentication.Infrastructure.Services
{
    public class ValidatorDatabaseChecker(IdentityDbContext dbContext) : IDatabaseChecks
    {
        #region [ Fields ]

        private readonly IdentityDbContext _dbContext = dbContext;

        #endregion

        #region [ Public Methods ]

        public async Task<bool> IsEmailUniqueAsync(string email, CancellationToken cancellationToken)
        {
            return !await _dbContext.Users.AnyAsync(u => u.Email == email, cancellationToken);
        }

        public async Task<bool> IsUsernameUniqueAsync(string username, CancellationToken cancellationToken)
        {
            return !await _dbContext.Users.AnyAsync(u => u.UserName == username, cancellationToken);
        }

        #endregion
    }
}