namespace AppyNox.Services.Base.Application.Interfaces.Caches;

public interface ICacheService
{
    #region [ Public Methods ]

    Task<string?> GetCachedValueAsync(string key);

    Task SetCachedValueAsync(string key, string value, TimeSpan? expiry = null);

    #endregion
}