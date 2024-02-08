using Application.Shared;
using Domain.Tasks;

namespace Application.PullRequests.Commands.Close;

public record ClosePullRequestCommand(Guid UserId, Guid PullRequestId) : ICommand<PullRequest>;