
using Application.Shared;


namespace Application.PullRequests.Commands.UserAssignment;

public record AssignUsersToPullRequestCommand(Guid Id, Guid UserId,
    List<Guid> AssigneeIds): ICommand<Guid>;