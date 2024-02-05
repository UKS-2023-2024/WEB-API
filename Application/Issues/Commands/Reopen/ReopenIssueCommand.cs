using Application.Shared;

namespace Application.Issues.Commands.Reopen;

public record ReopenIssueCommand(Guid CreatorId, Guid IssueId): ICommand<Guid>;