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
    private readonly IRepositoryRepository _repositoryRepository;

    public FindRepositoryPullRequestsQueryHandler(IPullRequestRepository pullRequestRepository,
        IRepositoryMemberRepository repositoryMemberRepository,
        IRepositoryRepository repositoryRepository)
    {
        _pullRequestRepository = pullRequestRepository;
        _repositoryMemberRepository = repositoryMemberRepository;
        _repositoryRepository = repositoryRepository;
    }

    public async Task<List<PullRequest>> Handle(FindRepositoryPullRequestsQuery request, CancellationToken cancellationToken)
    {
        var repository = _repositoryRepository.Find(request.RepositoryId);
        Repository.ThrowIfDoesntExist(repository);
        if (repository!.IsPrivate)
        {
            var repositoryMember =
                await _repositoryMemberRepository.FindByUserIdAndRepositoryId(request.UserId, request.RepositoryId);
            RepositoryMember.ThrowIfDoesntExist(repositoryMember);
        }
        var pullRequests = await _pullRequestRepository.FindAllByRepositoryId(request.RepositoryId);
        return pullRequests;
    }
}