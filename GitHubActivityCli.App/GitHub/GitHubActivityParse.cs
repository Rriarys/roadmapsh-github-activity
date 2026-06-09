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

    // Calculates and displays summarized activity statistics
    public static void DisplaySummaryActivity(JsonDocument activityJson)
    {
        var rawEvents = activityJson.RootElement.EnumerateArray();

        var createdRepoNames = new HashSet<string>();
        var pushedRepoNames = new HashSet<string>();
        var openedPullRepoNames = new HashSet<string>();
        var commentedRepoNames = new HashSet<string>();
        var starredRepoNames = new HashSet<string>();

        int pushedCount = 0;
        int openedPullCount = 0;
        int commentedCount = 0;

        foreach (var evt in rawEvents)
        {
            var eventType = evt.GetProperty("type").GetString();
            var repoName = evt.GetProperty("repo").GetProperty("name").GetString() ?? "unknown";

            // Cleans up repository name from user prefix if present
            var shortRepoName = repoName.Contains('/') ? repoName.Split('/')[1] : repoName;

            switch (eventType)
            {
                case "CreateEvent":
                    if (evt.TryGetProperty("payload", out var createPayload) &&
                        createPayload.TryGetProperty("ref_type", out var refTypeProp) &&
                        refTypeProp.GetString() == "repository")
                    {
                        createdRepoNames.Add(shortRepoName);
                    }
                    break;

                case "PushEvent":
                    pushedCount++;
                    pushedRepoNames.Add(shortRepoName);
                    break;

                case "PullRequestEvent":
                    if (evt.TryGetProperty("payload", out var prPayload) &&
                        prPayload.TryGetProperty("action", out var prActionProp) &&
                        prActionProp.GetString() == "opened")
                    {
                        openedPullCount++;
                        openedPullRepoNames.Add(shortRepoName);
                    }
                    break;

                case "IssueCommentEvent":
                    commentedCount++;
                    commentedRepoNames.Add(shortRepoName);
                    break;

                case "WatchEvent":
                    starredRepoNames.Add(shortRepoName);
                    break;
            }
        }

        if (createdRepoNames.Count > 0)
        {
            Console.WriteLine($"Created {createdRepoNames.Count} new {(createdRepoNames.Count == 1 ? "repository" : "repositories")} called {string.Join(" & ", createdRepoNames)}");
        }

        if (pushedCount > 0)
        {
            Console.WriteLine($"Pushed {pushedCount} new {(pushedCount == 1 ? "change" : "changes")} in {(pushedRepoNames.Count == 1 ? "repository" : "repositories")} {string.Join(" & ", pushedRepoNames)}");
        }

        if (openedPullCount > 0)
        {
            Console.WriteLine($"Opened {openedPullCount} new pull {(openedPullCount == 1 ? "request" : "requests")} in {(openedPullRepoNames.Count == 1 ? "repository" : "repositories")} {string.Join(" & ", openedPullRepoNames)}");
        }

        if (commentedCount > 0)
        {
            Console.WriteLine($"Added {commentedCount} new {(commentedCount == 1 ? "comment" : "comments")} in {(commentedRepoNames.Count == 1 ? "repository" : "repositories")} {string.Join(" & ", commentedRepoNames)}");
        }

        if (starredRepoNames.Count > 0)
        {
            Console.WriteLine($"Starred {string.Join(" & ", starredRepoNames)}");
        }

        Console.WriteLine();
    }
}