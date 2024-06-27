using AppyNox.Services.Base.Application.Extensions;
using AppyNox.Services.Base.Application.Interfaces.Loggers;
using AppyNox.Services.Base.Core.Constants;
using AppyNox.Services.Base.Infrastructure.Extensions;
using AppyNox.Services.Base.Infrastructure.Services.LoggerService;
using AppyNox.Services.Sso.Infrastructure.Hubs;
using AppyNox.Services.Sso.Infrastructure.Localization;
using AppyNox.Services.Sso.Server.UI.Components;
using AppyNox.Services.Sso.Server.UI.Middlewares;
using Microsoft.Extensions.Localization;
using MudBlazor.Services;
using Serilog;
using AppyNox.Services.Sso.Application;
using AppyNox.Services.Sso.Infrastructure;
using AppyNox.Services.Sso.Server.UI;
using AppyNox.Services.Sso.Application.Localization;

var builder = WebApplication.CreateBuilder(args);

#region [ Configuration Services ]

await builder.AddConsulConfiguration("SsoServerUi");
var configuration = builder.Configuration;


#endregion

#region [ Logger Setup ]

builder.Host.UseSerilog((context, services, config) =>
    config.ReadFrom.Configuration(context.Configuration)
          .ReadFrom.Services(services)
          .Enrich.FromLogContext()
);
builder.Services.AddSingleton<INoxApiLogger, NoxApiLogger>();

#region [ Logger for Before DI Initialization ]

Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .CreateLogger();

var loggerFactory = LoggerFactory.Create(builder =>
{
    builder.AddSerilog();
});
var logger = loggerFactory.CreateLogger<INoxLogger>();
NoxLogger noxLogger = new(logger, "SsoServerUiHost");

#endregion

#endregion

#region [ Dependency Injection For Layers ]

noxLogger.LogInformation("Registering DI's for layers.");
builder.Services.AddSsoApplication(configuration)
    .AddSsoInfrastructure(configuration, noxLogger, true)
    .AddServerUI(builder.Configuration);
noxLogger.LogInformation("Registering DI's for layers completed.");

#endregion

var app = builder.Build();

#region [ Localization Services ]

IStringLocalizerFactory localizerFactory = app.Services.GetRequiredService<IStringLocalizerFactory>();
localizerFactory.InitializeNoxApplicationLocalizationService();
localizerFactory.InitializeNoxInfrastructureLocalizationService();

CommonResources.Initialize(localizerFactory);

#endregion

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.UseAntiforgery();


app.MapHub<NoxHub>("/noxhub");

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();
