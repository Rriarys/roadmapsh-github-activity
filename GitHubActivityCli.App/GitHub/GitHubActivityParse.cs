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
                    var commitCount = payload.TryGetProperty("commits", out var commits) ? commits.GetArrayLength() : 0;
                    var reference = payload.TryGetProperty("ref", out var refProp) && refProp.ValueKind != JsonValueKind.Null
                        ? refProp.GetString()
                        : "unknown";

                    Console.WriteLine($"[PushEvent] Repo: {repoName}, Commits: {commitCount}, Branch: {reference}");
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

    public static void PrettyParseAndDisplayActivity(JsonDocument activityJson)
    {
        var events = activityJson.RootElement.EnumerateArray();

        foreach (var evt in events)
        {
            var eventType = evt.GetProperty("type").GetString();
            var repoName = evt.GetProperty("repo").GetProperty("name").GetString();

            if (!evt.TryGetProperty("payload", out var payload))
            {
                continue;
            }

            switch (eventType)
            {
                case "PushEvent":
                    var commitCount = payload.TryGetProperty("commits", out var commits) ? commits.GetArrayLength() : 0;
                    var reference = payload.TryGetProperty("ref", out var refProp) && refProp.ValueKind != JsonValueKind.Null
                        ? refProp.GetString()
                        : "unknown";

                    Console.WriteLine($"- Pushed {commitCount} commit(s) to {repoName} on {reference}");
                    break;

                case "IssuesEvent":
                    var issueAction = payload.TryGetProperty("action", out var actionProp)
                        ? actionProp.GetString()
                        : "unknown";
                    issueAction ??= "unknown";

                    var issueVerb = issueAction switch
                    {
                        "opened" => "Opened",
                        "closed" => "Closed",
                        "reopened" => "Reopened",
                        "labeled" => "Added label to",
                        "assigned" => "Assigned",
                        _ => char.ToUpper(issueAction[0]) + issueAction[1..]
                    };

                    Console.WriteLine($"- {issueVerb} an issue in {repoName}");
                    break;

                case "CreateEvent":
                    var refType = payload.TryGetProperty("ref_type", out var rType) ? rType.GetString() : "repository";
                    var refName = payload.TryGetProperty("ref", out var rProp) && rProp.ValueKind != JsonValueKind.Null
                        ? rProp.GetString()
                        : "repository";

                    Console.WriteLine(refType == "repository"
                        ? $"- Created a new repository {repoName}"
                        : $"- Created a new {refType} {refName} in {repoName}");
                    break;

                case "DeleteEvent":
                    var deletedRefType = payload.TryGetProperty("ref_type", out var dType) ? dType.GetString() : "unknown";
                    var deletedRefName = payload.TryGetProperty("ref", out var dProp) && dProp.ValueKind != JsonValueKind.Null
                        ? dProp.GetString()
                        : "repository";

                    Console.WriteLine($"- Deleted {deletedRefType} {deletedRefName} in {repoName}");
                    break;

                case "IssueCommentEvent":
                    var issueCommentTitle = payload.TryGetProperty("issue", out var issueCommentProp) &&
                                            issueCommentProp.TryGetProperty("title", out var issueCommentTitleProp)
                        ? issueCommentTitleProp.GetString()
                        : "unknown";

                    Console.WriteLine($"- Commented on issue \"{issueCommentTitle}\" in {repoName}");
                    break;
            }
        }
    }
}