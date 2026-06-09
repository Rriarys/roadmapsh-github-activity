namespace GitHubActivityCli.App.Cli;

internal class UserInput
{
    public static string GetGitHubUsername()
    {
        while (true)
        {
            Console.Write("Enter GitHub username to fetch activity (or type 'exit' to quit): ");
            var username = Console.ReadLine()?.Trim().ToLower();
            if (username == "exit")
            {
                return "exit";
            }

            if (string.IsNullOrEmpty(username))
            {
                Console.WriteLine("\nUsername cannot be empty. Please try again.\n");
                continue;
            }
            if (!UserInputValidation.IsValidGitHubUsername(username))
            {
                Console.WriteLine("\nInvalid GitHub username. Usernames can only contain alphanumeric characters or hyphens, cannot have multiple consecutive hyphens, and must be between 1 and 39 characters. Please try again.\n");
                continue;
            }
            return username;
        }
    }
}
