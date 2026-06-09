# GitHub Activity CLI

A simple command-line application that fetches a GitHub user's public activity and displays it in a readable format.

This is a small educational pet project created as part of the roadmap.sh project ideas pool:
https://roadmap.sh/projects/github-user-activity

The application retrieves recent public events from the GitHub API, displays them in a structured format, and generates a short activity summary.

## Project Purpose

This project was built to practice:

* working with REST APIs
* HTTP requests with `HttpClient`
* JSON deserialization
* handling external API responses
* command-line application development in C#
* error handling and validation

## Requirements

* .NET 10 SDK

Check installed SDKs:

```bash
dotnet --list-sdks
```

## Build

From the repository root:

```bash
dotnet build GitHubActivityCli.App
```

## Run

From the repository root:

```bash
dotnet run --project GitHubActivityCli.App
```

After startup, the application opens an interactive CLI session.

## Usage

Enter a GitHub username when prompted:

```text
Enter GitHub username to fetch activity (or type 'exit' to quit):
```

Example:

```text
Enter GitHub username to fetch activity (or type 'exit' to quit): Rriarys
```

## Supported Event Types

The application currently supports the following GitHub event types:

* `PushEvent`
* `IssuesEvent`
* `PullRequestEvent`
* `CreateEvent`
* `DeleteEvent`
* `IssueCommentEvent`

## Example Session

```text
Enter GitHub username to fetch activity (or type 'exit' to quit): Rriarys

Getting Rriarys's activity...

[PushEvent] Repo: Rriarys/roadmapsh-github-activity, Branch: refs/heads/master
[IssuesEvent] Repo: Rriarys/roadmapsh-github-activity, Action: closed, Issue: Documentation
[PullRequestEvent] Repo: Rriarys/roadmapsh-github-activity, Action: opened, Pull Request: unknown
[IssueCommentEvent] Repo: Rriarys/roadmapsh-github-activity, Issue: CLI Entry Point and Argument Parsing

Summarized activity statistics...

Pushed 10 new changes in repository roadmapsh-github-activity
Opened 1 new pull request in repository roadmapsh-github-activity
Added 1 new comment in repository roadmapsh-github-activity
```

## Error Handling

The application handles common failures gracefully:

* missing or empty username input
* user not found
* network errors
* GitHub API errors
* invalid or unexpected JSON responses

If an error occurs, the application prints a clear message and continues running without crashing.

## Notes

* Uses the public GitHub REST API
* No authentication is required
* Only public user activity can be retrieved
* Unsupported event types are safely ignored
