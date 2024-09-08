using Microsoft.Extensions.Logging.Abstractions;

using Orleans.TestingHost;

using QueryHive.Silo.Googler;

namespace QueryHive.Grains.Tests;

public class GooglerGrainTests
{
    private const string SampleSearchTerm = "Orleans";
    private const string ExpectedGoogleSearchPageLink = "https://www.google.com/?q=Orleans";
    private const string ExpectedGoogleSearchResultLink = "https://www.google.com/search?q=Orleans";
    [Fact]
    public async Task CreateGoogleSearchPageLink_ShouldReturnValidUri()
    {

        var builder = new TestClusterBuilder();
        var cluster = builder.Build();
        cluster.Deploy();

        // Arrange
        var grain = cluster.GrainFactory.GetGrain<IGooglerGrain>(SampleSearchTerm);

        // Act
        var result = await grain.CreateGoogleSearchPageLink();

        // Assert
        Assert.NotNull(result);
        Assert.Equal(ExpectedGoogleSearchPageLink, result.ToString());
    }

    [Fact]
    public async Task CreateGoogleSearchResultLink_ShouldReturnValidUri()
    {

        var builder = new TestClusterBuilder();
        var cluster = builder.Build();
        cluster.Deploy();

        // Arrange
        var grain = cluster.GrainFactory.GetGrain<IGooglerGrain>(SampleSearchTerm);

        // Act
        var result = await grain.CreateGoogleSearchResultLink();

        // Assert
        Assert.NotNull(result);
        Assert.Equal(ExpectedGoogleSearchResultLink, result.ToString());
    }
}
