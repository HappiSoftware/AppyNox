using AppyNox.Services.Base.Application.ExceptionExtensions.Base;
using Consul;
using MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using System.Net;
using System.Text;
using System.Text.Json;
using Winton.Extensions.Configuration.Consul;

namespace AppyNox.Services.Base.Infrastructure.Extensions
{
    public static class ApplicationBuilderExtensions
    {
        #region [ Fields ]

        private static readonly JsonSerializerOptions _jsonSerializerOptions = new JsonSerializerOptions() { PropertyNameCaseInsensitive = true };

        #endregion

        #region [ Public Methods ]

        /// <summary>
        /// Adds and loads configuration settings from Consul for a specified microservice. This method initializes a Consul
        /// client, uploads local configuration files (appsettings or ocelot) to the Consul KV store, and configures the application
        /// to use Consul as an additional configuration source. It's designed for use during application startup to integrate
        /// Consul for centralized configuration management.
        /// </summary>
        /// <param name="builder">The host builder for the application.</param>
        /// <param name="microServiceName">The name of the microservice, used for structuring Consul KV paths.</param>
        /// <param name="configurationFile">The configuration file name (default is "appsettings").</param>
        public static async Task AddConsulConfiguration(this IHostApplicationBuilder builder, string microServiceName, string configurationFile = "appsettings")
        {
            IConfiguration configuration = builder.Configuration;
            string environmentName = builder.Environment.EnvironmentName;
            Uri consulUri = new(configuration["ConsulConfiguration:Address"] ?? "http://localhost:8500");

            IConsulClient consulClient = new ConsulClient(config =>
            {
                config.Address = consulUri;
            });

            await consulClient.UploadConfigurationsToConsulAsync(environmentName, microServiceName, configurationFile);

            builder.Configuration.AddConsul(
                $"{microServiceName}/{environmentName}/",
                options =>
                {
                    options.ConsulConfigurationOptions = cco => { cco.Address = consulUri; };
                    options.Optional = false;
                    options.ReloadOnChange = true;
                    options.OnLoadException = exceptionContext =>
                    {
                        throw new NoxApplicationException(exceptionContext.Exception, $"Failed to load configuration from Consul for '{microServiceName}' in '{environmentName}' environment");
                    };
                });
        }

        public static void AddMassTransitWithRabbitMq(this IHostApplicationBuilder builder)
        {
            builder.Services.AddMassTransit(busConfigurator =>
            {
                busConfigurator.UsingRabbitMq((context, configurator) =>
                {
                    configurator.Host(new Uri(builder.Configuration["MessageBroker:Host"]!), h =>
                    {
                        h.Username(builder.Configuration["MessageBroker:Username"]!);
                        h.Password(builder.Configuration["MessageBroker:Password"]!);
                    });

                    configurator.ConfigureEndpoints(context);
                });
            });
        }

        #endregion

        #region [ Private Methods ]

        /// <summary>
        /// Uploads configuration settings (appsettings or ocelot) for a specified environment to the Consul KV store. This method
        /// reads the local JSON configuration file, deserializes it, and uploads each setting to the Consul KV store under a key
        /// constructed from the microservice name and environment. It's used for centralizing configuration management in
        /// distributed systems.
        /// </summary>
        /// <param name="consulClient">The Consul client used for interacting with the Consul KV store.</param>
        /// <param name="environmentName">The name of the environment (e.g., Development, Production).</param>
        /// <param name="microServiceName">The name of the microservice, used for structuring Consul KV paths.</param>
        /// <param name="configurationFile">The configuration file name to be uploaded (e.g., "appsettings", "ocelot").</param>
        private static async Task UploadConfigurationsToConsulAsync(this IConsulClient consulClient, string environmentName, string microServiceName, string configurationFile)
        {
            string appSettingsPath = $"{configurationFile}.{environmentName}.json";
            if (!File.Exists(appSettingsPath))
            {
                throw new FileNotFoundException($"'{configurationFile}' file could not found for '{microServiceName}' in '{environmentName}' environment");
            }

            var appsettings = JsonSerializer.Deserialize<Dictionary<string, object>>(
                File.ReadAllText(appSettingsPath),
                _jsonSerializerOptions
                ) ?? throw new InvalidOperationException($"Failed to deserialize {configurationFile} for '{microServiceName}' in '{environmentName}' environment");

            foreach (var setting in appsettings)
            {
                string key = $"{microServiceName}/{environmentName}/{setting.Key}";
                string value = setting.Value.ToString() ?? string.Empty;
                byte[] valueBytes = Encoding.UTF8.GetBytes(value);

                var kvPair = new KVPair(key) { Value = valueBytes };
                try
                {
                    WriteResult success = await consulClient.KV.Put(kvPair);
                    if (success.StatusCode != HttpStatusCode.OK)
                    {
                        throw new Exception($"Failed to store the key-value pair to Consul KV for '{microServiceName}' in '{environmentName}'.");
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception($"An error occurred while trying to upload configurations to Consul for '{microServiceName}' in '{environmentName}'.", ex);
                }
            }
        }

        #endregion
    }
}