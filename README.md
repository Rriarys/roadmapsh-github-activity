# GitHub Activity CLI

A simple command-line application that fetches a GitHub user's public activity and prints it in a readable format.

## Usage

```bash
GitHubActivityCli.App <username>
```

## Examples

```bash
GitHubActivityCli.App Rriarys
```

```bash
GitHubActivityCli.App octocat
```

## Supported event types

- `PushEvent`
- `IssuesEvent`
- `PullRequestEvent`
- `CreateEvent`
- `DeleteEvent`
- `IssueCommentEvent`

## Error handling

The application handles common failures gracefully:

- missing or empty username input
- user not found
- network errors
- GitHub API errors
- invalid or unexpected JSON responses

If an error occurs, the application prints a clear message and exits without crashing