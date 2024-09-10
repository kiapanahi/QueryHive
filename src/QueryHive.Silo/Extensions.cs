using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

using QueryHive.Silo.Googler;

namespace QueryHive.Silo;
public static class Extensions
{
    public static IHostApplicationBuilder AddQueryHive(this IHostApplicationBuilder builder)
    {
        builder.AddKeyedRedisClient("hive-redis");

        builder.UseOrleans();

        builder.Services.AddSingleton<GooglerMetrics>();

        return builder;
    }
}
