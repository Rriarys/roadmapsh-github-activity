using System.Text.Json;

namespace GitHubActivityCli.App.GitHub;

internal class GitHubActivityClient
{
    // GET https://api.github.com/users/{username}/events

    public async Task<JsonDocument?> GetUserActivityAsync(string username)
    {
        using var httpClient = new HttpClient();
        httpClient.DefaultRequestHeaders.UserAgent.ParseAdd("GitHubActivityCliApp");

        var url = $"https://api.github.com/users/{username}/events";

        try
        {
            var response = await httpClient.GetAsync(url);

            if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                Console.WriteLine($"\nUser '{username}' not found\n");
                return null;
            }

            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync();
            return JsonDocument.Parse(content);
        }
        catch (HttpRequestException ex)
        {
            Console.WriteLine($"\nError fetching user activity: {ex.Message}\n");
            return null;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"\nUnexpected error: {ex.Message}\n");
            return null;
        }
    }
}
