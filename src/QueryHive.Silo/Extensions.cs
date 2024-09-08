using Microsoft.Extensions.Hosting;

namespace QueryHive.Silo;
public static class Extensions
{
    public static IHostApplicationBuilder AddQueryHive(this IHostApplicationBuilder builder)
    {
        builder.AddKeyedRedisClient("hive-redis");

        builder.UseOrleans();

        return builder;
    }
}
