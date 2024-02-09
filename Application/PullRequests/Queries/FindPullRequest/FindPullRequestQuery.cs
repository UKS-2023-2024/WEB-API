using Application.Shared;
using Domain.Tasks;

namespace Application.PullRequests.Queries.FindPullRequest;

public record FindPullRequestQuery(Guid PullRequestId): IQuery<PullRequest>;