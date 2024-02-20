using Application.Shared;
using Domain.Tasks;

namespace Application.PullRequests.Queries.FindRepositoryPullRequests;

public record FindRepositoryPullRequestsQuery(Guid UserId, Guid RepositoryId): IQuery<List<PullRequest>>;