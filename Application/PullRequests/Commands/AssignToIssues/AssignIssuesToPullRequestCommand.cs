
using Application.Shared;


namespace Application.PullRequests.Commands.Update;

public record AssignIssuesToPullRequestCommand(Guid Id, Guid UserId,
    List<Guid> IssuesIds): ICommand<Guid>;