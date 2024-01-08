namespace AppyNox.Services.Authentication.Application.Validators.SharedRules
{
    public interface IDatabaseChecks
    {
        #region Public Methods

        Task<bool> IsEmailUniqueAsync(string email, CancellationToken cancellationToken);

        Task<bool> IsUsernameUniqueAsync(string username, CancellationToken cancellationToken);

        #endregion
    }
}