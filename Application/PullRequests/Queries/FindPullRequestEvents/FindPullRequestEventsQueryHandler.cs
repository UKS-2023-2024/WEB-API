using Application.Shared;
using Domain.Tasks;
using Domain.Tasks.Interfaces;

namespace Application.PullRequests.Queries.FindPullRequestEvents;

public class FindPullRequestEventsQueryHandler : IQueryHandler<FindPullRequestEventsQuery, List<Event>>
{
    private readonly IPullRequestRepository _pullRequestRepository;


    public FindPullRequestEventsQueryHandler(IPullRequestRepository pullRequestRepository)
    {
        _pullRequestRepository = pullRequestRepository;
    }

    public async Task<List<Event>> Handle(FindPullRequestEventsQuery request, CancellationToken cancellationToken)
    {
        var pullRequest = _pullRequestRepository.Find(request.PullRequestId);
        PullRequest.ThrowIfDoesntExist(pullRequest);
        var sortedEvents = pullRequest!.Events.OrderBy(e => e.CreatedAt).ToList();
        return sortedEvents;
    }
}