using System.Net;
using System.Text.Json;

namespace GitHubActivityCli.App.GitHub;

internal sealed class GitHubActivityResult
{
    public HttpStatusCode? StatusCode { get; }
    public JsonDocument? Activity { get; }

    public bool IsSuccess => Activity is not null;
    public bool IsNotFound => StatusCode == HttpStatusCode.NotFound;

    public GitHubActivityResult(HttpStatusCode statusCode)
    {
        StatusCode = statusCode;
    }

    public GitHubActivityResult(JsonDocument activity)
    {
        Activity = activity;
    }
}