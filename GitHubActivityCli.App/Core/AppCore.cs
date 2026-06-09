using GitHubActivityCli.App.Cli;
using GitHubActivityCli.App.GitHub;
using System.Net;

namespace GitHubActivityCli.App.Core;

internal class AppCore
{
    public static async Task RunAsync()
    {
        while (true)
        {
            string username = UserInput.GetGitHubUsername();

            if (username == "exit")
            {
                break;
            }

            Console.WriteLine($"\nGetting {username}'s activity...\n");

            var client = new GitHubActivityClient();

            var result = await client.GetUserActivityAsync(username);

            if (result.IsSuccess)
            {
                // Checks if the activity array is empty
                if (result.Activity!.RootElement.GetArrayLength() == 0)
                {
                    Console.WriteLine($"No recent activity found for user '{username}'\n");
                }
                else
                {
                    GitHubActivityParse.ParseAndDisplayActivity(result.Activity!);

                    Console.WriteLine();
                }
            }
            else if (result.IsNotFound)
            {
                Console.WriteLine($"User '{username}' not found\n");
            }
            else
            {
                Console.WriteLine($"\nUnexpected error while fetching {username}'s activity.\n");
            }
        }
    }
}