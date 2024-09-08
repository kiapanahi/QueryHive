namespace QueryHive.Silo.Googler;

[Alias("QueryHive.Silo.Googler.IGooglerGrain")]
public interface IGooglerGrain : IGrainWithStringKey
{
    [Alias("create-search-page-link")]
    ValueTask<Uri> CreateGoogleSearchPageLink();


    [Alias("create-search-result-link")]
    ValueTask<Uri> CreateGoogleSearchResultLink();
}
