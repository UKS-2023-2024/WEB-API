
using Application.Shared;


namespace Application.PullRequests.Commands.IssueAssignment;

public record AssignIssuesToPullRequestCommand(Guid Id, Guid UserId,
    List<Guid> IssuesIds): ICommand<Guid>;