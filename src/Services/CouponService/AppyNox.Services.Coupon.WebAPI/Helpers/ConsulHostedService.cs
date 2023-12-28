using AppyNox.Services.Base.API.Logger;
using AppyNox.Services.Base.Domain.Common;
using AppyNox.Services.Base.Domain.Common.HttpStatusCodes;
using AppyNox.Services.Coupon.WebAPI.ExceptionExtensions.Base;
using Consul;

namespace AppyNox.Services.Coupon.WebAPI.Helpers;

public class ConsulHostedService : IHostedService
{
    #region [ Fields ]

    private readonly IConsulClient _consulClient;

    private readonly IConfiguration _configuration;

    private readonly INoxApiLogger _logger;

    #endregion

    #region [ Events ]

    public event Func<Task>? OnConsulConnectionFailed;

    #endregion

    #region [ Public Constructors ]

    public ConsulHostedService(IConsulClient consulClient, IConfiguration configuration, INoxApiLogger logger)
    {
        _consulClient = consulClient;
        _configuration = configuration;
        _logger = logger;
    }

    #endregion

    #region [ Public Methods ]

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        try
        {
            var serviceConfig = _configuration.GetSection("consul").Get<ConsulConfig>() ??
            throw new CouponBaseException("Consul configuration is not defined. Service will not be discovered.", (int)NoxServerErrorResponseCodes.ServiceUnavailable);

            var registration = new AgentServiceRegistration
            {
                ID = serviceConfig.ServiceId,
                Name = serviceConfig.ServiceName,
                Address = serviceConfig.ServiceHost,
                Port = serviceConfig.ServicePort,
                Tags = serviceConfig.Tags
            };

            _logger.LogInformation($"Registering service with Consul: {registration.Name}");

            await _consulClient.Agent.ServiceDeregister(registration.ID, cancellationToken);
            await _consulClient.Agent.ServiceRegister(registration, cancellationToken);

            _logger.LogInformation($"Registering service with Consul is successfull: {registration.Name}");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while attempting to register to Consul service.");
            OnConsulConnectionFailed?.Invoke();
        }
    }

    public async Task StopAsync(CancellationToken cancellationToken)
    {
        var serviceConfig = _configuration.GetSection("consul").Get<ConsulConfig>();

        if (serviceConfig == null)
        {
            _logger.LogWarning("Consul configuration is not found. Service will not be deregistered from Consul.");
            return;
        }

        var registration = new AgentServiceRegistration { ID = serviceConfig.ServiceId };

        _logger.LogInformation($"Deregistering service from Consul: {registration.ID}");

        await _consulClient.Agent.ServiceDeregister(registration.ID, cancellationToken);

        _logger.LogInformation($"Deregistering service from Consul is successfull: {registration.ID}");
    }

    #endregion
}