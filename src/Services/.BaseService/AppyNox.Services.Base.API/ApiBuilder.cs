using AppyNox.Services.Base.API.Constants;
using AppyNox.Services.Base.API.Extensions;
using AppyNox.Services.Base.API.Middleware;
using AppyNox.Services.Base.API.Middleware.Options;
using AppyNox.Services.Base.Infrastructure.HostedServices;
using AutoMapper;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Localization;

namespace AppyNox.Services.Base.API;

public class ApiOptions
{
#nullable disable
    public IEnumerable<string> Versions { get; set; } = [NoxVersions.v1_0];
    public Action<IStringLocalizerFactory> LocalizationServices { get; set; }
    public bool PerformAutoMapperChecks { get; set; } = true;
    public bool UseConsul { get; set; } = true;

#nullable enable
}

public static class ApiBuilder
{
    public static void ConfigureNoxApi(this WebApplication app, Action<ApiOptions> configureOptions)
    {
        ApiOptions options = new();
        configureOptions(options);

        app.ConfigureLocalization(options);
        app.ConfigurePipeline(options);

        if(options.UseConsul)
        {
            var consulHostedService = app.Services.GetServices<IHostedService>()
            .OfType<ConsulHostedService>()
            .First();

            consulHostedService.OnConsulConnectionFailed += (Exception ex) =>
            {
                var lifeTime = app.Services.GetService<IHostApplicationLifetime>();
                lifeTime?.StopApplication();
                return Task.CompletedTask;
            };
        }

        if(options.PerformAutoMapperChecks)
        {
            PerformAutoMapperChecks(app);
        }

        app.MapDefaultEndpoints();
    }

    private static void ConfigureLocalization(this WebApplication app, ApiOptions options)
    {
        IStringLocalizerFactory localizerFactory = app.Services.GetRequiredService<IStringLocalizerFactory>();
        localizerFactory.AddNoxLocalizationServices();

        options.LocalizationServices?.Invoke(localizerFactory);
    }

    private static void ConfigurePipeline(this WebApplication app, ApiOptions options)
    {
        string serviceName = app.Configuration["Consul:ServiceName"] 
            ?? throw new InvalidOperationException("Consul:ServiceName was empty!");

        app.UseSwagger();
        app.UseSwaggerUI(c =>
        {
            foreach (var version in options.Versions)
            {
                c.SwaggerEndpoint($"/swagger/v{version}/swagger.json", $"{serviceName} v{version}");
            }
        });

        app.UseRequestLocalization();

        app.UseNoxContext();

        app.UseNoxResponseWrapper(new NoxResponseWrapperOptions
        {
            ApiVersion = options.Versions.Last()
        });

        app.UseHttpsRedirection();

        app.UseAuthentication();

        app.UseAuthorization();

        app.MapControllers();

        app.UseMiddleware<QueryParameterValidateMiddleware>();

        app.UseHealthChecks("/api/health");
    }

    private static void PerformAutoMapperChecks(this WebApplication app)
    {
        var mapper = app.Services.GetRequiredService<IMapper>();
        mapper.ConfigurationProvider.AssertConfigurationIsValid();
    }
}