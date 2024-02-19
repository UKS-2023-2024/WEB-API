using Application.Shared;
using Domain.Auth;
using Domain.Auth.Interfaces;
using Domain.Branches;
using Domain.Branches.Interfaces;
using Domain.Repositories.Interfaces;
using Domain.Shared.Interfaces;

namespace Application.Branches.Queries.ListCommits;

public class ListCommitsQueryHandler: IQueryHandler<ListCommitsQuery, List<CommitContent>>
{
    private readonly IGitService _gitService;
    private readonly IBranchRepository _branchRepository;
    private readonly IRepositoryRepository _repositoryRepository;
    
    public ListCommitsQueryHandler(
        IGitService gitService, 
        IBranchRepository branchRepository, 
        IRepositoryRepository repositoryRepository
        )
    {
        _gitService = gitService;
        _branchRepository = branchRepository;
        _repositoryRepository = repositoryRepository;
    }
    
    public async Task<List<CommitContent>> Handle(ListCommitsQuery request, CancellationToken cancellationToken)
    {
        var branch = await _branchRepository.FindById(request.Branch);
        Branch.ThrowIfDoesntExist(branch); 
        
        var repository = _repositoryRepository.Find(branch!.RepositoryId);

        return await _gitService.ListBranchCommits(repository!, branch);
    }
}