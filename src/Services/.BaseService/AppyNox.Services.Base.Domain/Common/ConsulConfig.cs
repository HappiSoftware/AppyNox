namespace AppyNox.Services.Base.Domain.Common;

public class ConsulConfig
{
    #region [ Properties ]

    public string ServiceId { get; set; }

    public string ServiceName { get; set; }

    public string Scheme { get; set; }

    public string ServiceHost { get; set; }

    public int ServicePort { get; set; }

    public string[] Tags { get; set; }

    public string HealthCheckUrl { get; set; }

    public int HealthCheckIntervalSeconds { get; set; }

    public int HealthCheckTimeoutSeconds { get; set; }

    #endregion
}