using AppyNox.Services.Base.Core.Constants;
using AppyNox.Services.Sso.Application.Interfaces;
using AppyNox.Services.Sso.Server.UI.Hubs;
using AppyNox.Services.Sso.Server.UI.Middlewares;
using AppyNox.Services.Sso.Server.UI.Services;
using AppyNox.Services.Sso.Server.UI.Services.Layout;
using AppyNox.Services.Sso.Server.UI.Services.UserPreferences;
using Microsoft.AspNetCore.HttpOverrides;
using MudBlazor;
using MudBlazor.Services;
using MudExtensions.Services;

namespace AppyNox.Services.Sso.Server.UI;

public static class DependencyInjection
{
    public static IServiceCollection AddServerUI(this IServiceCollection services, IConfiguration config)
    {
        services.AddRazorComponents().AddInteractiveServerComponents();
        services.AddSignalR();
        services.AddCascadingAuthenticationState();

        services.AddMudServices(config =>
        {
            config.SnackbarConfiguration.PositionClass = Defaults.Classes.Position.BottomCenter;
            config.SnackbarConfiguration.NewestOnTop = false;
            config.SnackbarConfiguration.ShowCloseIcon = true;
            config.SnackbarConfiguration.VisibleStateDuration = 3000;
            config.SnackbarConfiguration.HideTransitionDuration = 500;
            config.SnackbarConfiguration.ShowTransitionDuration = 500;
            config.SnackbarConfiguration.SnackbarVariant = Variant.Filled;
            config.SnackbarConfiguration.PreventDuplicates = false;

        })
            .AddMudExtensions()
            .AddMudPopoverService()
            .AddMudBlazorSnackbar()
            .AddMudBlazorDialog();


        string[] supportedCultures = LocalizationConstants.SupportedLanguages.Select(x => x.Code).ToArray();

        services
            .AddScoped<LocalizationCookiesMiddleware>()
                    .Configure<RequestLocalizationOptions>(options =>
                    {
                        options.AddSupportedUICultures(supportedCultures);
                        options.AddSupportedCultures(supportedCultures);
                        options.FallBackToParentUICultures = true;
                    })
                    .AddLocalization()
            .AddScoped<IApplicationHubWrapper, ServerHubWrapper>()
            .AddScoped<HubClient>()
            .AddScoped<LayoutService>()
            .AddScoped<IUserPreferencesService, UserPreferencesService>()
            .AddScoped<IdentityRedirectManager>();

        services.AddControllers();

        services.AddProblemDetails();
        services.AddHealthChecks();
        services.AddHttpContextAccessor();


        services.Configure<ForwardedHeadersOptions>(options =>
        {
            options.ForwardedHeaders =
                ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto;
        });
        return services;
    }
}