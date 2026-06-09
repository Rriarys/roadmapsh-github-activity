using System.Text.Json;

namespace GitHubActivityCli.App.GitHub;

internal class GitHubActivityParse
{
    public static void ParseAndDisplayActivity(JsonDocument activityJson)
    {
        // Enumerate through the array of GitHub events
        var events = activityJson.RootElement.EnumerateArray();

        foreach (var evt in events)
        {
            // Extract core properties present in every event
            var eventType = evt.GetProperty("type").GetString();
            var repoName = evt.GetProperty("repo").GetProperty("name").GetString();

            // Skip or log if the payload property is completely missing
            if (!evt.TryGetProperty("payload", out var payload))
            {
                Console.WriteLine($"[{eventType}] Repo: {repoName} (No payload data)");
                continue;
            }

            switch (eventType)
            {
                case "PushEvent":
                    // Safely check if the commits array exists to avoid KeyNotFoundException
                    int commitCount = payload.TryGetProperty("commits", out var commits)
                        ? commits.GetArrayLength()
                        : 0;

                    Console.WriteLine($"[PushEvent] Repo: {repoName}, Commits: {commitCount}");
                    break;

                case "IssuesEvent":
                case "PullRequestEvent":
                    // Combined identical logic for handling action properties
                    var action = payload.TryGetProperty("action", out var actionProp)
                        ? actionProp.GetString()
                        : "unknown";

                    Console.WriteLine($"[{eventType}] Repo: {repoName}, Action: {action}");
                    break;

                case "CreateEvent":
                case "DeleteEvent":
                    var refType = payload.TryGetProperty("ref_type", out var rType) ? rType.GetString() : "unknown";

                    // The 'ref' property can be null if the event relates to the repository itself
                    var reference = payload.TryGetProperty("ref", out var rProp) && rProp.ValueKind != JsonValueKind.Null
                        ? rProp.GetString()
                        : "repository";

                    Console.WriteLine($"[{eventType}] Repo: {repoName}, Ref Type: {refType}, Ref: {reference}");
                    break;

                default:
                    Console.WriteLine($"[OtherEvent] Repo: {repoName}, Event Type: {eventType}");
                    break;
            }
        }
    }
}