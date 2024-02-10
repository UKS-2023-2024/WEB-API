
using Application.Shared;


namespace Application.PullRequests.Commands.MilestoneUnassignment;

public record UnassignMilestoneToPullRequestCommand(Guid Id, Guid UserId): ICommand<Guid>;