using GitHubActivityCli.App.Cli;
using GitHubActivityCli.App.GitHub;

namespace GitHubActivityCli.App.Core;

internal class AppCore
{
    public static async Task RunAsync()
    {
        string username = UserInput.GetGitHubUsername();
        Console.WriteLine(username);

        Console.WriteLine("\nGetting user activity...\n");

        var client = new GitHubActivityClient();
        var activity = await client.GetUserActivityAsync(username);

        if (activity != null)
        {
            GitHubActivityParse.ParseAndDisplayActivity(activity);
        }
        else
        {
            Console.WriteLine("Unexpected error");
        }
    }
}
