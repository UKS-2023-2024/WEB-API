using Application.Shared;
using Domain.Repositories;
using Domain.Repositories.Interfaces;
using Domain.Tasks;
using Domain.Tasks.Interfaces;

namespace Application.PullRequests.Queries.FindPullRequest;

public class FindPullRequestQueryHandler: IQueryHandler<FindPullRequestQuery, PullRequest>
{
    private readonly IPullRequestRepository _pullRequestRepository;
    private readonly IRepositoryMemberRepository _repositoryMemberRepository;

    public FindPullRequestQueryHandler(IPullRequestRepository pullRequestRepository,
        IRepositoryMemberRepository repositoryMemberRepository)
    {
        _pullRequestRepository = pullRequestRepository;
        _repositoryMemberRepository = repositoryMemberRepository;
    }

    public async Task<PullRequest> Handle(FindPullRequestQuery request, CancellationToken cancellationToken)
    {
        var pullRequest = _pullRequestRepository.Find(request.PullRequestId);
        PullRequest.ThrowIfDoesntExist(pullRequest);
        return pullRequest;
    }
}