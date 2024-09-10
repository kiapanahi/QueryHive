using System.Diagnostics.Metrics;

namespace QueryHive.Silo.Googler;
internal sealed class GooglerMetrics
{
    private static readonly string Version = typeof(GooglerMetrics).Assembly.GetName().Version!.ToString(3);

    private readonly Meter _meter;

    public GooglerMetrics(IMeterFactory factory)
    {
        _meter = factory.Create("queryhive.searches.googler", version: Version);
    }


}
