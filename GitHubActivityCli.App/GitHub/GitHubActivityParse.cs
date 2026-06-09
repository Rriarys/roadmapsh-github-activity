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

                    Console.WriteLine($"[IssuesEvent] Repo: {repoName}, Action: {issueAction}");
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
                    var issueTitle = payload.TryGetProperty("issue", out var issueProp) &&
                                     issueProp.TryGetProperty("title", out var titleProp)
                        ? titleProp.GetString()
                        : "unknown";

                    Console.WriteLine($"[IssueCommentEvent] Repo: {repoName}, Issue: {issueTitle}");
                    break;
            }
        }
    }   
}