
using System.Globalization;
using System.Text;

using Microsoft.Extensions.Logging;

using QueryHive.Silo.Models;

namespace QueryHive.Silo.Googler;

internal sealed partial class GooglerGrain(
    [PersistentState("search-term", "queries")] IPersistentState<SearchTermModel> state,
    ILogger<GooglerGrain> logger) : Grain, IGooglerGrain
{

    private static readonly CompositeFormat GoogleSearchPageLink = CompositeFormat.Parse("https://www.google.com/?q={0}");
    private static readonly CompositeFormat GoogleSearchResultLink = CompositeFormat.Parse("https://www.google.com/search?q={0}");
    private readonly IPersistentState<SearchTermModel> _state = state;
    private readonly ILogger<GooglerGrain> _logger = logger;

    public async ValueTask<string> CreateGoogleSearchPageLink()
    {
        if (_state.RecordExists)
        {
            Log.StateExists(_logger, _state.Etag, _state.State);

            if (string.IsNullOrEmpty(_state.State.GoogleSearchPageLink))
            {

                var googleSearchPageLink = CreateUri(GoogleSearchPageLink);
                //await _state.ClearStateAsync().ConfigureAwait(false);
                _state.State.GoogleSearchPageLink = googleSearchPageLink;
                await _state.WriteStateAsync().ConfigureAwait(false);
                //await _state.ReadStateAsync().ConfigureAwait(false);
            }
            var stateUri = _state.State.GoogleSearchPageLink;
            return stateUri;
        }
        var uri = CreateUri(GoogleSearchPageLink);
        _state.State = new SearchTermModel
        {
            Term = this.GetPrimaryKeyString(),
            GoogleSearchPageLink = uri,
            GoogleSearchResultLink = string.Empty,
            CreationDate = DateTimeOffset.UtcNow
        };
        await _state.WriteStateAsync().ConfigureAwait(false);
        return uri;
    }

    public async ValueTask<string> CreateGoogleSearchResultLink()
    {
        if (_state.RecordExists)
        {
            Log.StateExists(_logger, _state.Etag, _state.State);

            if (string.IsNullOrEmpty(_state.State.GoogleSearchResultLink))
            {

                var googleSearchResultPage = CreateUri(GoogleSearchResultLink);
                //await _state.ClearStateAsync().ConfigureAwait(false);
                _state.State.GoogleSearchResultLink = googleSearchResultPage;
                await _state.WriteStateAsync().ConfigureAwait(false);
                //await _state.ReadStateAsync().ConfigureAwait(false);
            }
            var stateUri = _state.State.GoogleSearchResultLink;
            return stateUri;
        }
        var uri = CreateUri(GoogleSearchResultLink);
        _state.State = new SearchTermModel
        {
            Term = this.GetPrimaryKeyString(),
            GoogleSearchPageLink = string.Empty,
            GoogleSearchResultLink = uri,
            CreationDate = DateTimeOffset.UtcNow
        };
        await _state.WriteStateAsync().ConfigureAwait(false);
        return uri;
    }

    private string CreateUri(CompositeFormat googleUri)
    {
        var query = this.GetPrimaryKeyString();
        var result = string.Format(CultureInfo.InvariantCulture, googleUri, query);
        if (Uri.TryCreate(result, UriKind.Absolute, out _))
        {
            Log.UriCreated(_logger);
            return result;
        }

        Log.ErrorCreatingRedirectUri(_logger);
        throw new InvalidOperationException("Failed to create a valid URI.");
    }

    private static partial class Log
    {
        [LoggerMessage(LogLevel.Information,
            EventId = 11,
            EventName = "StateAlreadyExists",
            Message = "State already exists with ETAG: {etag} and value: {state}")]
        public static partial void StateExists(ILogger logger, string etag, SearchTermModel state);

        [LoggerMessage(LogLevel.Debug,
            "Creating redirect URI",
            EventId = 10,
            EventName = "RedirectUriCreated")]
        public static partial void UriCreated(ILogger logger);


        [LoggerMessage(LogLevel.Error,
            "Failed to create redirect URI",
            EventId = 20,
            EventName = "ErrorCreatingRedirectUri")]
        public static partial void ErrorCreatingRedirectUri(ILogger logger);
    }
}
