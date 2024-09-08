namespace QueryHive.Tests;

public class AppHostTests
{
    [Fact]
    public async Task EnsureRedisResourceInitialized()
    {
        // Arrange
        var appHost = await DistributedApplicationTestingBuilder.CreateAsync<Projects.QueryHive_AppHost>();
        appHost.Services.ConfigureHttpClientDefaults(clientBuilder =>
        {
            clientBuilder.AddStandardResilienceHandler();
        });
        // To output logs to the xUnit.net ITestOutputHelper, consider adding a package from https://www.nuget.org/packages?q=xunit+logging

        await using var app = await appHost.BuildAsync();
        var resourceNotificationService = app.Services.GetRequiredService<ResourceNotificationService>();
        await app.StartAsync();

        var timeoutToken = new CancellationTokenSource(TimeSpan.FromSeconds(5));

        //Assert
        await resourceNotificationService.WaitForResourceAsync("hive-redis", KnownResourceStates.Running, timeoutToken.Token);
    }
}
