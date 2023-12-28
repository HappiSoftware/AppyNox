using AppyNox.Services.Authentication.WebAPI.ExceptionExtensions.Base;
using AppyNox.Services.Base.API.Logger;
using AppyNox.Services.Base.Domain.Common;
using AppyNox.Services.Base.Domain.Common.HttpStatusCodes;
using Consul;

namespace AppyNox.Services.Authentication.WebAPI.Helpers;

public class ConsulHostedService(IConsulClient consulClient, IConfiguration configuration/*, INoxApiLogger logger*/) : IHostedService
{
    #region [ Fields ]

    private readonly IConsulClient _consulClient = consulClient;

    private readonly IConfiguration _configuration = configuration;

    //private readonly INoxApiLogger _logger = logger;
    // TODO: Behlul Uncomment this line when the logger is implemented

    #endregion

    #region [ Events ]

    public event Func<Task>? OnConsulConnectionFailed;

    #endregion

    #region [ Public Methods ]

    /// <summary>
    ///Starts the Consul service, registering it with the Consul agent.
    /// </summary>
    /// <param name="cancellationToken">Token to monitor for cancellation requests.</param>
    /// <returns>A Task representing the asynchronous operation</returns>
    /// <exception cref="InvalidOperationException">Thrown when appsettings section for Consul is not provided or wrong</exception>
    public async Task StartAsync(CancellationToken cancellationToken)
    {
        try
        {
            var serviceConfig = _configuration.GetSection("consul").Get<ConsulConfig>() ??
            throw new InvalidOperationException("Consul configuration is not defined. Service will not be discovered.");

            var registration = new AgentServiceRegistration
            {
                ID = serviceConfig.ServiceId,
                Name = serviceConfig.ServiceName,
                Address = serviceConfig.ServiceHost,
                Port = serviceConfig.ServicePort,
                Tags = serviceConfig.Tags
            };

            //_logger.LogInformation($"Registering service with Consul: {registration.Name}");

            await _consulClient.Agent.ServiceDeregister(registration.ID, cancellationToken);
            await _consulClient.Agent.ServiceRegister(registration, cancellationToken);

            //_logger.LogInformation($"Registering service with Consul is successfull: {registration.Name}");
        }
        catch (Exception ex)
        {
            //_logger.LogError(ex, "An error occurred while attempting to register to Consul service.");
            OnConsulConnectionFailed?.Invoke();
        }
    }

    /// <summary>
    /// Stops the Consul service, deregistering it from the Consul agent.
    /// </summary>
    /// <param name="cancellationToken">Token to monitor for cancellation requests.</param>
    /// <returns>A Task representing the asynchronous operation.</returns>
    public async Task StopAsync(CancellationToken cancellationToken)
    {
        var serviceConfig = _configuration.GetSection("consul").Get<ConsulConfig>();

        if (serviceConfig == null)
        {
            //_logger.LogWarning("Consul configuration is not found. Service will not be deregistered from Consul.");
            return;
        }

        var registration = new AgentServiceRegistration { ID = serviceConfig.ServiceId };

        //_logger.LogInformation($"Deregistering service from Consul: {registration.ID}");

        await _consulClient.Agent.ServiceDeregister(registration.ID, cancellationToken);

        //_logger.LogInformation($"Deregistering service from Consul is successfull: {registration.ID}");
    }

    #endregion
}