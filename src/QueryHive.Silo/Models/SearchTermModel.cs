namespace QueryHive.Silo.Models;

[GenerateSerializer]
[Immutable]
[Alias("QueryHive.Silo.Models.SearchTermModel")]
public sealed record class SearchTermModel(string Term, DateTimeOffset Timestamp);
