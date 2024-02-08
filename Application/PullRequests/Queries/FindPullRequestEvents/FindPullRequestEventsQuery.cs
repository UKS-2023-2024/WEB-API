using Application.Shared;
using Domain.Tasks;

namespace Application.PullRequests.Queries.FindPullRequestEvents;

public record FindPullRequestEventsQuery(Guid UserId, Guid RepositoryId, Guid PullRequestId) : IQuery<List<Event>>;