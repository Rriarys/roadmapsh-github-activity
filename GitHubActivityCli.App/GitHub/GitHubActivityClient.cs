using System.Net;
using System.Text.Json;

namespace GitHubActivityCli.App.GitHub;

internal class GitHubActivityClient
{
    // GET https://api.github.com/users/{username}/events

    public async Task<GitHubActivityResult> GetUserActivityAsync(string username)
    {
        using var httpClient = new HttpClient();
        httpClient.DefaultRequestHeaders.UserAgent.ParseAdd("GitHubActivityCliApp");

        var url = $"https://api.github.com/users/{username}/events";

        try
        {
            var response = await httpClient.GetAsync(url);

            if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                return new GitHubActivityResult(HttpStatusCode.NotFound);
            }

            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync();
            return new GitHubActivityResult(JsonDocument.Parse(content));
        }
        catch (HttpRequestException ex)
        {
            Console.WriteLine($"\nError fetching user activity: {ex.Message}\n");
            return new GitHubActivityResult(HttpStatusCode.ServiceUnavailable);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"\nUnexpected error: {ex.Message}\n");
            return new GitHubActivityResult(HttpStatusCode.InternalServerError);
        }
    }
}
