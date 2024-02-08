using Application.Shared;
using Domain.Tasks;

namespace Application.PullRequests.Queries.FindPullRequest;

public record FindPullRequestQuery(Guid UserId, Guid PullRequestId, Guid RepositoryId): IQuery<PullRequest>;