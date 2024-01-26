using Microsoft.Extensions.Localization;

namespace AppyNox.Services.Base.Core.Extensions
{
    public static class LocalizedStringExtensions
    {
        #region [ Public Methods ]

        public static string Format(this LocalizedString resource, params object[] args)
        {
            return string.Format(resource, args);
        }

        #endregion
    }
}