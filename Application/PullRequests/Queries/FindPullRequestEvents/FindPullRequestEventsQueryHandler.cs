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

        return pullRequest!.Events;
    }
}