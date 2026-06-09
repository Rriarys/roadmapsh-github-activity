using System.Text.Json;

namespace GitHubActivityCli.App.GitHub;

internal class GitHubActivityParse
{
    public static void ParseAndDisplayActivity(JsonDocument activityJson)
    {
        var events = activityJson.RootElement.EnumerateArray();

        foreach (var evt in events)
        {
            var eventType = evt.GetProperty("type").GetString();
            var repoName = evt.GetProperty("repo").GetProperty("name").GetString();

            if (!evt.TryGetProperty("payload", out var payload))
            {
                Console.WriteLine($"[{eventType}] Repo: {repoName} (No payload data)");
                continue;
            }

            switch (eventType)
            {
                case "PushEvent":
                    var reference = payload.TryGetProperty("ref", out var refProp) && refProp.ValueKind != JsonValueKind.Null
                        ? refProp.GetString()
                        : "unknown";

                    Console.WriteLine($"[PushEvent] Repo: {repoName}, Branch: {reference}");
                    break;

                case "IssuesEvent":
                    var issueAction = payload.TryGetProperty("action", out var actionProp)
                        ? actionProp.GetString()
                        : "unknown";

                    var issueTitle = payload.TryGetProperty("issue", out var issueProp) &&
                                     issueProp.TryGetProperty("title", out var titleProp)
                        ? titleProp.GetString()
                        : "unknown";

                    Console.WriteLine($"[IssuesEvent] Repo: {repoName}, Action: {issueAction}, Issue: {issueTitle}");
                    break;

                case "PullRequestEvent":
                    var pullRequestAction = payload.TryGetProperty("action", out var pullActionProp)
                        ? pullActionProp.GetString()
                        : "unknown";

                    var pullRequestTitle = payload.TryGetProperty("pull_request", out var pullRequestProp) &&
                                           pullRequestProp.TryGetProperty("title", out var pullTitleProp)
                        ? pullTitleProp.GetString()
                        : "unknown";

                    Console.WriteLine($"[PullRequestEvent] Repo: {repoName}, Action: {pullRequestAction}, Pull Request: {pullRequestTitle}");
                    break;

                case "CreateEvent":
                case "DeleteEvent":
                    var refType = payload.TryGetProperty("ref_type", out var rType) ? rType.GetString() : "unknown";
                    var refName = payload.TryGetProperty("ref", out var rProp) && rProp.ValueKind != JsonValueKind.Null
                        ? rProp.GetString()
                        : "repository";

                    Console.WriteLine($"[{eventType}] Repo: {repoName}, Ref Type: {refType}, Ref: {refName}");
                    break;

                case "IssueCommentEvent":
                    var issueCommentTitle = payload.TryGetProperty("issue", out var issueCommentProp) &&
                                            issueCommentProp.TryGetProperty("title", out var issueCommentTitleProp)
                        ? issueCommentTitleProp.GetString()
                        : "unknown";

                    Console.WriteLine($"[IssueCommentEvent] Repo: {repoName}, Issue: {issueCommentTitle}");
                    break;
            }
        }
    }
}