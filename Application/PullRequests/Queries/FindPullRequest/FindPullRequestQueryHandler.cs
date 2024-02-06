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
        var repositoryMember =
            await _repositoryMemberRepository.FindByUserIdAndRepositoryId(request.UserId, request.RepositoryId);
        RepositoryMember.ThrowIfDoesntExist(repositoryMember);
        var pullRequest = await _pullRequestRepository.FindByIdAndRepositoryId(request.RepositoryId, request.PullRequestId);
        PullRequest.ThrowIfDoesntExist(pullRequest);
        return pullRequest;
    }
}