namespace GitHubActivityCli.App.Cli;

internal class UserInputValidation
{
    public static bool IsValidGitHubUsername(string username)
    {
        // GitHub usernames can only contain alphanumeric characters or hyphens,
        // cannot have multiple consecutive hyphens, and must be between 1 and 39 characters.
        var regex = new System.Text.RegularExpressions.Regex(@"^[a-zA-Z0-9](?:[a-zA-Z0-9-]{0,37}[a-zA-Z0-9])?$");
        return regex.IsMatch(username);
    }
}
