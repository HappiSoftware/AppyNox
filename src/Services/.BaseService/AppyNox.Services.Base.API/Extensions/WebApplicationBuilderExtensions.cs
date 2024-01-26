using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Localization;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace AppyNox.Services.Base.API.Extensions
{
    public static class WebApplicationBuilderExtensions
    {
        #region [ Public Methods ]

        public static void ConfigureLocalization(this IHostApplicationBuilder builder, string resourcesPath = "", params string[] supportedCultures)
        {
            builder.Services.AddLocalization(options => options.ResourcesPath = resourcesPath);

            builder.Services.Configure<RequestLocalizationOptions>(options =>
            {
                if (supportedCultures.Length <= 0)
                {
                    supportedCultures = ["en-US", "tr-TR"];
                }

                options.SetDefaultCulture(supportedCultures[0])
                    .AddSupportedCultures(supportedCultures)
                    .AddSupportedUICultures(supportedCultures);

                options.RequestCultureProviders.Insert(0, new CustomRequestCultureProvider(context =>
                {
                    // Read the language preference from the Accept-Language header
                    var acceptLanguageHeader = context.Request.Headers.AcceptLanguage.ToString();

                    // Parse the header to get the preferred language
                    var preferredLanguage = acceptLanguageHeader.Split(',').FirstOrDefault();

                    // Validate and map the preferred language to one of the supported cultures
                    var isValidCulture = supportedCultures.Any(c => string.Equals(c, preferredLanguage, StringComparison.OrdinalIgnoreCase));
                    ProviderCultureResult? result = isValidCulture
                        ? new ProviderCultureResult(preferredLanguage)
                        : new ProviderCultureResult(supportedCultures[0]);

                    return Task.FromResult<ProviderCultureResult?>(result);
                }));
            });
        }

        #endregion
    }
}