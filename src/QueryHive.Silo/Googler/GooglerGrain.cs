
using System.Globalization;
using System.Text;

using Microsoft.Extensions.Logging;

namespace QueryHive.Silo.Googler;

internal sealed partial class GooglerGrain(ILogger<GooglerGrain> logger) : Grain, IGooglerGrain
{

    private static readonly CompositeFormat GoogleSearchPageLink = CompositeFormat.Parse("https://www.google.com/?q={0}");
    private static readonly CompositeFormat GoogleSearchResultLink = CompositeFormat.Parse("https://www.google.com/search?q={0}");

    private readonly ILogger<GooglerGrain> _logger = logger;

    public ValueTask<Uri> CreateGoogleSearchPageLink()
    {
        return CreateUri(GoogleSearchPageLink);
    }

    public ValueTask<Uri> CreateGoogleSearchResultLink()
    {
        return CreateUri(GoogleSearchResultLink);
    }

    private ValueTask<Uri> CreateUri(CompositeFormat googleUri)
    {
        var query = this.GetPrimaryKeyString();
        var result = string.Format(CultureInfo.InvariantCulture, googleUri, query);
        if (Uri.TryCreate(result, UriKind.Absolute, out var uri))
        {
            Log.UriCreated(_logger);
            return ValueTask.FromResult(uri);
        }

        Log.ErrorCreatingRedirectUri(_logger);
        throw new InvalidOperationException("Failed to create a valid URI.");
    }

    private static partial class Log
    {
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
