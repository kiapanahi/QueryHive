using Orleans.TestingHost;

using QueryHive.Silo.Googler;

namespace QueryHive.Grains.Tests.Grains;

[Collection(ClusterCollectionFixture.Name)]
public class GooglerGrainTests(ClusterFixture fixture)
{
    private const string SampleSearchTerm = "Orleans";
    private const string ExpectedGoogleSearchPageLink = "https://www.google.com/?q=Orleans";
    private const string ExpectedGoogleSearchResultLink = "https://www.google.com/search?q=Orleans";

    private readonly TestCluster _cluster = fixture.Cluster;

    [Fact]
    public async Task CreateGoogleSearchPageLink_ShouldReturnValidUri()
    {
        // Arrange
        var grain = _cluster.GrainFactory.GetGrain<IGooglerGrain>(SampleSearchTerm);

        // Act
        var result = await grain.CreateGoogleSearchPageLink();

        // Assert
        Assert.NotNull(result);
        Assert.Equal(ExpectedGoogleSearchPageLink, result.ToString());
    }

    [Fact]
    public async Task CreateGoogleSearchResultLink_ShouldReturnValidUri()
    {
        // Arrange
        var grain = _cluster.GrainFactory.GetGrain<IGooglerGrain>(SampleSearchTerm);

        // Act
        var result = await grain.CreateGoogleSearchResultLink();

        // Assert
        Assert.NotNull(result);
        Assert.Equal(ExpectedGoogleSearchResultLink, result.ToString());
    }
}
