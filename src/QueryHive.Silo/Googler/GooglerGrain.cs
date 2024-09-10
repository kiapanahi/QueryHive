
using System.Globalization;
using System.Text;

using Microsoft.Extensions.Logging;

using QueryHive.Silo.Models;

namespace QueryHive.Silo.Googler;

internal sealed partial class GooglerGrain(
    [PersistentState("search-term", "queries")] IPersistentState<SearchTermModel> state,
    ILogger<GooglerGrain> logger) : Grain, IGooglerGrain
{

    private static readonly CompositeFormat SearchPageTemplate = CompositeFormat.Parse("https://www.google.com/?q={0}");
    private static readonly CompositeFormat ResultPageTemplate = CompositeFormat.Parse("https://www.google.com/search?q={0}");

    private readonly IPersistentState<SearchTermModel> _state = state;
    private readonly ILogger<GooglerGrain> _logger = logger;

    public async ValueTask<string> CreateGoogleSearchPageLink()
    {
        if (_state.RecordExists)
        {
            Log.StateExists(_logger, _state.Etag, _state.State);

            if (_state.State.IsSearchLinkPopulated())
            {
                return _state.State.SearchPage;
            }

            var searchPageLink = CreateUri(SearchPageTemplate);
            //await _state.ClearStateAsync();
            _state.State.SetSearchLink(searchPageLink);
            //await _state.ReadStateAsync();
        }
        else
        {
            _state.State = new SearchTermModel
            {
                Term = this.GetPrimaryKeyString(),
                SearchPage = CreateUri(SearchPageTemplate),
                ResultPage = string.Empty,
                CreationDate = DateTimeOffset.UtcNow
            };
        }

        try
        {
            await _state.WriteStateAsync();
            return _state.State.SearchPage;
        }
        catch (Exception e)
        {
            Log.WriteStateThrew(_logger, e);
            throw;
        }
    }

    public async ValueTask<string> CreateGoogleSearchResultLink()
    {
        if (_state.RecordExists)
        {
            Log.StateExists(_logger, _state.Etag, _state.State);

            if (_state.State.IsResultLinkPopulated())
            {
                return _state.State.ResultPage;
            }

            var resultPageLink = CreateUri(ResultPageTemplate);
            //await _state.ClearStateAsync();
            _state.State.SetResultLink(resultPageLink);
            //await _state.ReadStateAsync();
        }
        else
        {
            _state.State = new SearchTermModel
            {
                Term = this.GetPrimaryKeyString(),
                SearchPage = string.Empty,
                ResultPage = CreateUri(ResultPageTemplate),
                CreationDate = DateTimeOffset.UtcNow
            };
        }

        try
        {
            await _state.WriteStateAsync();
            return _state.State.ResultPage;
        }
        catch (Exception e)
        {
            Log.WriteStateThrew(_logger, e);
            throw;
        }
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

        [LoggerMessage(LogLevel.Error,
            "Failed to write state",
            EventId = 31,
            EventName = "WriteStateThrew",
            Message = "Error while writing state")]
        public static partial void WriteStateThrew(ILogger logger, Exception ex);
    }
}
