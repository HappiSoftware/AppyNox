using AppyNox.Services.Base.Application.Interfaces.Loggers;
using AppyNox.Services.Base.Core.Common;
using AppyNox.Services.Base.Infrastructure.Exceptions.Base;
using Consul;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using System.Net;

namespace AppyNox.Services.Base.Infrastructure.HostedServices;

public class ConsulHostedService(IConsulClient consulClient, IConfiguration configuration, INoxInfrastructureLogger logger) : IHostedService
{
    #region [ Fields ]

    private readonly IConsulClient _consulClient = consulClient;

    private readonly ConsulConfiguration _consulConfig = configuration.GetSection("consul").Get<ConsulConfiguration>() ??
            throw new NoxInfrastructureException("Consul configuration is not defined. Service will not be discovered.", (int)NoxInfrastructureExceptionCode.DevelopmentError, (int)HttpStatusCode.ServiceUnavailable);

    private readonly INoxInfrastructureLogger _logger = logger;

    #endregion

    #region [ Events ]

    public event Func<Exception, Task>? OnConsulConnectionFailed;

    #endregion

    #region [ Public Methods ]

    /// <summary>
    /// Firstly deregisters once and then registers to Consul for avoiding multiple registering.
    /// </summary>
    /// <exception cref="NoxInfrastructureException">Thrown if unexpected error occurs.</exception>
    public async Task StartAsync(CancellationToken cancellationToken)
    {
        try
        {
            _logger.LogInformation("Starting Consul Hosted Service.");
            var registration = new AgentServiceRegistration
            {
                ID = _consulConfig.ServiceId,
                Name = _consulConfig.ServiceName,
                Address = _consulConfig.ServiceHost,
                Port = _consulConfig.ServicePort,
                Tags = _consulConfig.Tags
                //Check = new AgentServiceCheck
                //{
                //    HTTP = $"{_consulConfig.Scheme}://{_consulConfig.HealthCheckServiceHost ?? _consulConfig.ServiceHost}:{_consulConfig.ServicePort}/{_consulConfig.HealthCheckUrl}",
                //    Interval = TimeSpan.FromSeconds(_consulConfig.HealthCheckIntervalSeconds),
                //    Timeout = TimeSpan.FromSeconds(_consulConfig.HealthCheckTimeoutSeconds),
                //    DeregisterCriticalServiceAfter = TimeSpan.FromMinutes(1)
                //}
            };

            _logger.LogInformation($"Registering service with Consul: {registration.Name}");

            await _consulClient.Agent.ServiceDeregister(registration.ID, cancellationToken);
            await _consulClient.Agent.ServiceRegister(registration, cancellationToken);

            _logger.LogInformation($"Registering service with Consul is successful: {registration.Name}");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while attempting to register to Consul service.");
            OnConsulConnectionFailed?.Invoke(ex);
        }
    }

    /// <summary>
    /// Deregisters from Consul while application stopping.
    /// </summary>
    /// <exception cref="NoxInfrastructureException">Thrown if unexpected error occurs.</exception>
    public async Task StopAsync(CancellationToken cancellationToken)
    {
        var registration = new AgentServiceRegistration { ID = _consulConfig.ServiceId };

        _logger.LogInformation($"Deregistering service from Consul: {registration.ID}");

        try
        {
            await _consulClient.Agent.ServiceDeregister(registration.ID, cancellationToken);
            _logger.LogInformation($"Deregistering service from Consul is successful: {registration.ID}");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to deregister service from Consul.");
            throw;
        }
    }

    #endregion
}