using Application.Shared;
using Domain.Tasks;

namespace Application.PullRequests.Commands.Reopen;

public record ReopenPullRequestCommand(Guid UserId, Guid PullRequestId): ICommand<PullRequest>;