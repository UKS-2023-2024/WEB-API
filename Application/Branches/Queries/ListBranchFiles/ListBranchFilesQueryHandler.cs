using Application.Shared;
using Domain.Auth;
using Domain.Auth.Interfaces;
using Domain.Branches;
using Domain.Branches.Exceptions;
using Domain.Branches.Interfaces;
using Domain.Repositories.Interfaces;
using Domain.Shared.Interfaces;

namespace Application.Branches.Queries.ListBranchFiles;

public class ListBranchFilesQueryHandler: IQueryHandler<ListBranchFilesQuery, List<ContributionFile>>
{
    private readonly IGitService _gitService;
    private readonly IBranchRepository _branchRepository;
    private readonly IRepositoryRepository _repositoryRepository;
    
    public ListBranchFilesQueryHandler(
        IGitService gitService, 
        IBranchRepository branchRepository, 
        IRepositoryRepository repositoryRepository
        )
    {
        _gitService = gitService;
        _branchRepository = branchRepository;
        _repositoryRepository = repositoryRepository;
    }
    
    
    public async Task<List<ContributionFile>> Handle(ListBranchFilesQuery request, CancellationToken cancellationToken)
    {
        var branch = await _branchRepository.FindById(request.BranchId);
        Branch.ThrowIfDoesntExist(branch); 
        
        var repository = _repositoryRepository.Find(branch!.RepositoryId);
        
        return await _gitService.ListFolderContent(repository!, branch, request.Folder);
    }
}