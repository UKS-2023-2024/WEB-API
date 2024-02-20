using Application.PullRequests.Queries.GetDiffPreview;
using Application.Shared;
using Domain.Auth;
using Domain.Auth.Interfaces;
using Domain.Branches;
using Domain.Repositories;
using Domain.Repositories.Interfaces;
using Domain.Shared.Interfaces;
using Domain.Tasks;
using Domain.Tasks.Interfaces;

namespace Application.PullRequests.Queries.GetCommitPreview;

public class GetCommitPreviewQueryHandler: IQueryHandler<GetCommitPreviewQuery, List<CommitContent>>
{
    private readonly IGitService _gitService;
    private readonly IRepositoryRepository _repositoryRepository;
    private readonly IUserRepository _userRepository;
    private readonly IPullRequestRepository _pullRequestRepository;


    public GetCommitPreviewQueryHandler(
        IGitService gitService,
        IRepositoryRepository repositoryRepository,
        IUserRepository userRepository,
        IPullRequestRepository pullRequestRepository
    )
    {
        _gitService = gitService;
        _repositoryRepository = repositoryRepository;
        _userRepository = userRepository;
        _pullRequestRepository = pullRequestRepository;
    }
    
    
    public async Task<List<CommitContent>> Handle(GetCommitPreviewQuery request, CancellationToken cancellationToken)
    {
        var pullRequest = _pullRequestRepository.Find(request.Id);
        PullRequest.ThrowIfDoesntExist(pullRequest);
        
        var repository = _repositoryRepository.Find(pullRequest!.RepositoryId);
        Repository.ThrowIfDoesntExist(repository);

        var owner = await _repositoryRepository.FindRepositoryOwner(pullRequest.RepositoryId);
        User.ThrowIfDoesntExist(owner);

        return await _gitService.ListPrCommits(owner!, repository!, pullRequest!);
    }
}