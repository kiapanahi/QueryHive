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
    public required string SearchPage { get; set; }
    [Id(2)]
    public required string ResultPage { get; set; }
    [Id(3)]
    public DateTimeOffset CreationDate { get; set; }

    public override string ToString()
    {
        return JsonSerializer.Serialize(this);
    }

    public bool IsSearchLinkPopulated() => !string.IsNullOrEmpty(SearchPage);
    public bool IsResultLinkPopulated() => !string.IsNullOrEmpty(ResultPage);

    ///<summary>
    /// Sets the search link for the search term.
    /// </summary>
    /// <param name="link">The search link to set.</param>
    /// <exception cref="ArgumentException" >Thrown when the link is null or empty.</exception>
    public void SetSearchLink(string link)
    {
        ArgumentException.ThrowIfNullOrEmpty(link, nameof(link));

        SearchPage = link;
    }

    ///<summary>
    /// Sets the result link for the search term.
    /// </summary>
    /// <param name="link">The search link to set.</param>
    /// <exception cref="ArgumentException" >Thrown when the link is null or empty.</exception>
    public void SetResultLink(string link)
    {
        ArgumentException.ThrowIfNullOrEmpty(link, nameof(link));

        ResultPage = link;
    }
}
