using Application.Shared;
using Domain.Auth;
using Domain.Auth.Interfaces;
using Domain.Repositories;
using Domain.Repositories.Interfaces;
using Domain.Shared.Interfaces;
using Domain.Tasks;
using Domain.Tasks.Interfaces;

namespace Application.PullRequests.Queries.GetDiffPreview;

public class GetDiffPreviewQueryHandler: IQueryHandler<GetDiffPreviewQuery, string>
{
    private readonly IGitService _gitService;
    private readonly IRepositoryRepository _repositoryRepository;
    private readonly IUserRepository _userRepository;
    private readonly IPullRequestRepository _pullRequestRepository;


    public GetDiffPreviewQueryHandler(
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
    
    
    public async Task<string> Handle(GetDiffPreviewQuery request, CancellationToken cancellationToken)
    {
        var pullRequest = _pullRequestRepository.Find(request.Id);
        PullRequest.ThrowIfDoesntExist(pullRequest);
        
        var repository = _repositoryRepository.Find(pullRequest!.RepositoryId);
        Repository.ThrowIfDoesntExist(repository);

        var owner = await _repositoryRepository.FindRepositoryOwner(pullRequest.RepositoryId);
        User.ThrowIfDoesntExist(owner);

        return await _gitService.GetPrDiffPreview(owner!, repository!, pullRequest!);
    }
}