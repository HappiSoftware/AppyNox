using AppyNox.Services.Base.Domain.Common;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Configuration;
using AppyNox.Services.Base.Infrastructure.ExceptionExtensions.Base;
using Consul;
using AppyNox.Services.Base.Infrastructure.Logger;
using System.Net;

namespace AppyNox.Services.Base.Infrastructure.HostedServices;

public class ConsulHostedService : IHostedService
{
    #region [ Fields ]

    private readonly IConsulClient _consulClient;

    private readonly IConfiguration _configuration;

    private readonly ConsulConfig _consulConfig;

    private readonly INoxInfrastructureLogger _logger;

    #endregion

    #region [ Events ]

    public event Func<Exception, Task>? OnConsulConnectionFailed;

    #endregion

    #region [ Public Constructors ]

    public ConsulHostedService(IConsulClient consulClient, IConfiguration configuration, INoxInfrastructureLogger logger)
    {
        _consulClient = consulClient;
        _configuration = configuration;
        _logger = logger;
        _consulConfig = _configuration.GetSection("consul").Get<ConsulConfig>() ??
            throw new NoxInfrastructureException("Consul configuration is not defined. Service will not be discovered.", (int)HttpStatusCode.ServiceUnavailable);
    }

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

        await _consulClient.Agent.ServiceDeregister(registration.ID, cancellationToken);

        _logger.LogInformation($"Deregistering service from Consul is successful: {registration.ID}");
    }

    #endregion
}