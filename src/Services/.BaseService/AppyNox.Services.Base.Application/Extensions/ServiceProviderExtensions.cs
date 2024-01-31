using AppyNox.Services.Base.Application.Localization;
using Microsoft.Extensions.Localization;

namespace AppyNox.Services.Base.Application.Extensions
{
    public static class ServiceProviderExtensions
    {
        #region [ Public Methods ]

        public static void InitializeNoxApplicationLocalizationService(this IStringLocalizerFactory localizerFactory)
        {
            NoxApplicationResourceService.Initialize(localizerFactory);
        }

        #endregion
    }
}