using Application.Shared;
using Domain.Milestones.Interfaces;
using Domain.Repositories;
using Domain.Repositories.Interfaces;
using Domain.Tasks;
using Domain.Tasks.Enums;
using Domain.Tasks.Interfaces;

namespace Application.PullRequests.Queries.FindPullRequestEvents;

public class FindPullRequestEventsQueryHandler : IQueryHandler<FindPullRequestEventsQuery, List<Event>>
{
    private readonly IPullRequestRepository _pullRequestRepository;
    private readonly IRepositoryMemberRepository _repositoryMemberRepository;
    private readonly IMilestoneRepository _milestoneRepository;

    public FindPullRequestEventsQueryHandler(IPullRequestRepository pullRequestRepository,
        IRepositoryMemberRepository repositoryMemberRepository, IMilestoneRepository milestoneRepository)
    {
        _pullRequestRepository = pullRequestRepository;
        _repositoryMemberRepository = repositoryMemberRepository;
        _milestoneRepository = milestoneRepository;
    }

    public async Task<List<Event>> Handle(FindPullRequestEventsQuery request, CancellationToken cancellationToken)
    {
        var repositoryMember =
            await _repositoryMemberRepository.FindByUserIdAndRepositoryId(request.UserId, request.RepositoryId);
        RepositoryMember.ThrowIfDoesntExist(repositoryMember);
        var pullRequest = await _pullRequestRepository.FindByIdAndRepositoryId(request.RepositoryId, request.PullRequestId);
        PullRequest.ThrowIfDoesntExist(pullRequest);

        var events = pullRequest.Events;
        
        events = events.Select(e =>
        {
            if (e.EventType == EventType.PULL_REQUEST_ASSIGNED)
            {
                var ev = (AssignPullRequestEvent)e;
                var assignee = _repositoryMemberRepository.Find(ev.AssigneeId);
                ev.Assignee = assignee;
                return ev;
            }
            
            if (e.EventType == EventType.PULL_REQUEST_UNASSIGNED)
            {
                var ev = (UnnassignPullRequestEvent)e;
                var assignee = _repositoryMemberRepository.Find(ev.AssigneeId);
                ev.Assignee = assignee;
                return ev;
            }

            if (e.EventType == EventType.MILESTONE_ASSIGNED)
            {
                var ev = (AssignMilestoneEvent)e;
                var milestone = _milestoneRepository.Find(ev.MilestoneId);
                ev.Milestone = milestone;
                return ev;
            }
            
            if (e.EventType == EventType.MILESTONE_UNASSIGNED)
            {
                var ev = (UnassignMilestoneEvent)e;
                var milestone = _milestoneRepository.Find(ev.MilestoneId ?? new Guid());
                ev.Milestone = milestone;
                return ev;
            }

            return e;
        }).ToList();
        return events;
    }
}