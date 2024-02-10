
using Application.Shared;


namespace Application.PullRequests.Commands.MilestoneAssignment;

public record AssignMilestoneToPullRequestCommand(Guid Id, Guid UserId,
    Guid? MilestoneId): ICommand<Guid>;