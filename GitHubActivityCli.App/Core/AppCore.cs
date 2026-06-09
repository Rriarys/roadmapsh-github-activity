using GitHubActivityCli.App.Cli;
using GitHubActivityCli.App.GitHub;

namespace GitHubActivityCli.App.Core;

internal class AppCore
{
    public static async Task RunAsync()
    {
        bool isRunning = true;
        while (isRunning) 
        {
            string username = UserInput.GetGitHubUsername();

            if (username == "exit")
            {
                break;
            }

            Console.WriteLine($"\nGetting {username}'s activity...\n");

            var client = new GitHubActivityClient();
            var activity = await client.GetUserActivityAsync(username);

            if (activity != null)
            {
                GitHubActivityParse.ParseAndDisplayActivity(activity);
                Console.WriteLine("\n");
            }
            else
            {
                Console.WriteLine($"\nUnexpected error while fetching {username}'s activity.\n");
            }
        }

    }
}
