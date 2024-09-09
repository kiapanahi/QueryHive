using System.Text.Json;

namespace QueryHive.Silo.Models;

//[Immutable]
[GenerateSerializer]
[Alias("QueryHive.Silo.Models.SearchTermModel")]
public sealed class SearchTermModel
{
    [Id(0)]
    public required string Term { get; set; }
    [Id(1)]
    public required string GoogleSearchPageLink { get; set; }
    [Id(2)]
    public required string GoogleSearchResultLink { get; set; }
    [Id(3)]
    public DateTimeOffset CreationDate { get; set; }

    public override string ToString()
    {
        return JsonSerializer.Serialize(this);
    }
}
