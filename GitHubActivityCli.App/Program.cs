using GitHubActivityCli.App.Cli;
using GitHubActivityCli.App.GitHub;

string username = UserInput.GetGitHubUsername();
Console.WriteLine(username);

Console.WriteLine("\n-----------------------------------\n");

var client = new GitHubActivityClient();
var activity = await client.GetUserActivityAsync(username);
Console.WriteLine(activity);