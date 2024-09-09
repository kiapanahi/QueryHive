using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

using Orleans.TestingHost;

namespace QueryHive.Grains.Tests;

internal sealed class TestSiloBuilderConfigurator : ISiloConfigurator
{
    public void Configure(ISiloBuilder siloBuilder)
    {
        siloBuilder
        .UseLocalhostClustering()
        .AddMemoryGrainStorage("queries")
        .ConfigureServices(static services => services
            .AddLogging(logger => logger.ClearProviders().AddConsole()));
    }
}

internal sealed class TestClientBuilderConfigurator : IClientBuilderConfigurator
{
    public void Configure(IConfiguration configuration, IClientBuilder clientBuilder)
    {
        clientBuilder.UseLocalhostClustering();
    }
}
