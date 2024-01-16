namespace AppyNox.Services.Base.Core.Common;

/// <summary>
/// Configuration settings for Consul service registration and health checks.
/// </summary>
public class ConsulConfiguration
{
    #region [ Properties ]

    /// <summary>
    /// Gets or sets the unique service identifier.
    /// </summary>
    public string ServiceId { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the name of the service.
    /// </summary>
    public string ServiceName { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the scheme used for service communication (e.g., http, https).
    /// </summary>
    public string Scheme { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the host address of the service.
    /// </summary>
    public string ServiceHost { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the port on which the service runs.
    /// </summary>
    public int ServicePort { get; set; }

    /// <summary>
    /// Gets or sets the port on which the service runs.
    /// </summary>
    public string[]? Tags { get; set; }

    /// <summary>
    /// Gets or sets the URL for the service's health check endpoint.
    /// </summary>
    public string? HealthCheckUrl { get; set; }

    /// <summary>
    /// Gets or sets the interval in seconds between health checks.
    /// </summary>
    public int HealthCheckIntervalSeconds { get; set; }

    /// <summary>
    /// Gets or sets the timeout in seconds for health check responses.
    /// </summary>
    public int HealthCheckTimeoutSeconds { get; set; }

    #endregion
}