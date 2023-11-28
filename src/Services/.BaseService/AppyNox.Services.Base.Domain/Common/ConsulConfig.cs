namespace AppyNox.Services.Base.Domain.Common;

public class ConsulConfig
{
    #region [ Properties ]

    public string ServiceId { get; set; } = string.Empty;

    public string ServiceName { get; set; } = string.Empty;

    public string Scheme { get; set; } = string.Empty;

    public string ServiceHost { get; set; } = string.Empty;

    public int ServicePort { get; set; }

    public string[]? Tags { get; set; }

    public string? HealthCheckUrl { get; set; }

    public int HealthCheckIntervalSeconds { get; set; }

    public int HealthCheckTimeoutSeconds { get; set; }

    #endregion
}