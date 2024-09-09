namespace QueryHive.Grains.Tests;

[CollectionDefinition(Name)]
public sealed class ClusterCollectionFixture : ICollectionFixture<ClusterFixture>
{
    public const string Name = nameof(ClusterCollectionFixture);
}