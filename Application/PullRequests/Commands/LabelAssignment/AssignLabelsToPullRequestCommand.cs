
using Application.Shared;


namespace Application.PullRequests.Commands.LabelAssignment;

public record AssignLabelsToPullRequestCommand(Guid Id, Guid UserId,
    List<Guid> LabelIds): ICommand<Guid>;