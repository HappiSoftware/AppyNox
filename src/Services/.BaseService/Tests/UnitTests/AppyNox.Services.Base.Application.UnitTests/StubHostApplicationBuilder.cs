using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.Metrics;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace AppyNox.Services.Base.Application.UnitTests;

public class StubHostApplicationBuilder : IHostApplicationBuilder
{
    public IServiceCollection Services { get; } = new ServiceCollection();

    public IConfigurationManager Configuration => throw new NotImplementedException();

    public IHostEnvironment Environment => throw new NotImplementedException();

    public ILoggingBuilder Logging => throw new NotImplementedException();

    public IMetricsBuilder Metrics => throw new NotImplementedException();

    IDictionary<object, object> IHostApplicationBuilder.Properties => throw new NotImplementedException();

    public void ConfigureContainer<TContainerBuilder>(IServiceProviderFactory<TContainerBuilder> factory, Action<TContainerBuilder>? configure = null) where TContainerBuilder : notnull
    {
        throw new NotImplementedException();
    }
}
