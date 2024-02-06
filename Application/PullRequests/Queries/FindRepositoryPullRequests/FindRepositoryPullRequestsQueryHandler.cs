using Application.Shared;
using Domain.Repositories;
using Domain.Repositories.Interfaces;
using Domain.Tasks;
using Domain.Tasks.Interfaces;

namespace Application.PullRequests.Queries.FindRepositoryPullRequests;

public class FindRepositoryPullRequestsQueryHandler: IQueryHandler<FindRepositoryPullRequestsQuery,List<PullRequest>>
{
    private readonly IPullRequestRepository _pullRequestRepository;
    private readonly IRepositoryMemberRepository _repositoryMemberRepository;

    public FindRepositoryPullRequestsQueryHandler(IPullRequestRepository pullRequestRepository,
        IRepositoryMemberRepository repositoryMemberRepository)
    {
        _pullRequestRepository = pullRequestRepository;
        _repositoryMemberRepository = repositoryMemberRepository;
    }

    public async Task<List<PullRequest>> Handle(FindRepositoryPullRequestsQuery request, CancellationToken cancellationToken)
    {
        var repositoryMember =
            await _repositoryMemberRepository.FindByUserIdAndRepositoryId(request.UserId, request.RepositoryId);
        RepositoryMember.ThrowIfDoesntExist(repositoryMember);
        var pullRequests = await _pullRequestRepository.FindAllByRepositoryId(request.RepositoryId);
        return pullRequests;
    }
}