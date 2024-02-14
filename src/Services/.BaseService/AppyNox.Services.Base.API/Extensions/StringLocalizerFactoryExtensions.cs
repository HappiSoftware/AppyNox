using AppyNox.Services.Base.API.Localization;
using AppyNox.Services.Base.Application.Extensions;
using AppyNox.Services.Base.Infrastructure.Extensions;
using Microsoft.Extensions.Localization;

namespace AppyNox.Services.Base.API.Extensions
{
    public static class StringLocalizerFactoryExtensions
    {
        #region [ Public Methods ]

        public static void InitializeNoxApiLocalizationService(this IStringLocalizerFactory localizerFactory)
        {
            NoxApiResourceService.Initialize(localizerFactory);
        }

        public static void AddNoxLocalizationServices(this IStringLocalizerFactory localizerFactory)
        {
            localizerFactory.InitializeNoxApplicationLocalizationService();
            localizerFactory.InitializeNoxInfrastructureLocalizationService();
            localizerFactory.InitializeNoxApiLocalizationService();
        }

        #endregion
    }
}