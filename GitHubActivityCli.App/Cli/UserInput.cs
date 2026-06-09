namespace GitHubActivityCli.App.Cli;

internal class UserInput
{
    public static string GetGitHubUsername()
    {
        while (true)
        {
            Console.Write("Enter GitHub username: ");
            var username = Console.ReadLine()?.Trim();
            if (string.IsNullOrEmpty(username))
            {
                Console.WriteLine("Username cannot be empty. Please try again.");
                continue;
            }
            if (!UserInputValidation.IsValidGitHubUsername(username))
            {
                Console.WriteLine("Invalid GitHub username. Usernames can only contain alphanumeric characters or hyphens, cannot have multiple consecutive hyphens, and must be between 1 and 39 characters. Please try again.");
                continue;
            }
            return username;
        }
    }
}
