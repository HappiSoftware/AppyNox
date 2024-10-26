﻿using AppyNox.Services.Sso.Application.Validators.SharedRules;
using AppyNox.Services.Sso.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace AppyNox.Services.Sso.Infrastructure.Services
{
    public class ValidatorDatabaseChecker(IdentityDatabaseContext dbContext) : IDatabaseChecks
    {
        #region [ Fields ]

        private readonly IdentityDatabaseContext _dbContext = dbContext;

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