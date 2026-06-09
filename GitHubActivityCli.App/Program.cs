using GitHubActivityCli.App.Cli;
using GitHubActivityCli.App.GitHub;

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

Console.WriteLine("\nPretty user activity:\n");

if (activity != null)
{
    GitHubActivityParse.PrettyParseAndDisplayActivity(activity);
}
else
{
    Console.WriteLine("Unexpected error");
}