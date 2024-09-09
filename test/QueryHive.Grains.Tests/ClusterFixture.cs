using Orleans.TestingHost;

namespace QueryHive.Grains.Tests;

public sealed class ClusterFixture : IDisposable
{
    public TestCluster Cluster { get; }


    public ClusterFixture()
    {
        var builder = new TestClusterBuilder();
        // Silo configuration
        builder = builder.AddSiloBuilderConfigurator<TestSiloBuilderConfigurator>();
        // Client configuration
        builder = builder.AddClientBuilderConfigurator<TestClientBuilderConfigurator>();

        Cluster = builder.Build();
        Cluster.Deploy();
    }

    void IDisposable.Dispose() => Cluster.StopAllSilos();
}
