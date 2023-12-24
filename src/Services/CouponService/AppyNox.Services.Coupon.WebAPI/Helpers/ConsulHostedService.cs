﻿using AppyNox.Services.Base.Domain.Common;
using AppyNox.Services.Base.Domain.Common.HttpStatusCodes;
using AppyNox.Services.Coupon.WebAPI.ExceptionExtensions.Base;
using Consul;

namespace AppyNox.Services.Coupon.WebAPI.Helpers;

public class ConsulHostedService : IHostedService
{
    #region [ Fields ]

    private readonly IConsulClient _consulClient;

    private readonly IConfiguration _configuration;

    private readonly ILogger<ConsulHostedService> _logger;

    #endregion

    #region [ Public Constructors ]

    public ConsulHostedService(IConsulClient consulClient, IConfiguration configuration, ILogger<ConsulHostedService> logger)
    {
        _consulClient = consulClient;
        _configuration = configuration;
        _logger = logger;
    }

    #endregion

    #region [ Public Methods ]

    public async Task StartAsync(CancellationToken cancellationToken)
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

        var logMsg = $"Registering service with Consul: {registration.Name}";
        _logger.LogInformation("{Message}", logMsg);

        await _consulClient.Agent.ServiceDeregister(registration.ID, cancellationToken);
        await _consulClient.Agent.ServiceRegister(registration, cancellationToken);
    }

    public async Task StopAsync(CancellationToken cancellationToken)
    {
        var serviceConfig = _configuration.GetSection("consul").Get<ConsulConfig>();

        if (serviceConfig == null)
        {
            _logger.LogWarning("{Message}", "Consul configuration is not found. Service will not be deregistered from Consul.");
            return;
        }

        var registration = new AgentServiceRegistration { ID = serviceConfig.ServiceId };

        var logMsg = $"Deregistering service from Consul: {registration.ID}";
        _logger.LogInformation("{Message}", logMsg);

        await _consulClient.Agent.ServiceDeregister(registration.ID, cancellationToken);
    }

    #endregion
}