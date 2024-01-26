using Microsoft.Extensions.Localization;

namespace AppyNox.Services.Base.Core.Services
{
    public abstract class NoxResourceServiceBase(IStringLocalizer localizer)
    {
        #region [ Fields ]

        protected readonly IStringLocalizer Localizer = localizer;

        #endregion

        #region [ Protected Methods ]

        protected virtual LocalizedString GetString(string key)
        {
            return Localizer[key];
        }

        protected virtual string GetFormatted(string resourceString, params object[] args)
        {
            return string.Format(resourceString, args);
        }

        #endregion
    }
}