
using Application.Shared;


namespace Application.PullRequests.Commands.MilestoneUnassignment;

public record UnassignMilestoneFromPullRequestCommand(Guid Id, Guid UserId): ICommand<Guid>;