using Application.Shared;

namespace Application.Issues.Commands.Close;

public record CloseIssueCommand(Guid CreatorId, Guid IssueId): ICommand<Guid>;