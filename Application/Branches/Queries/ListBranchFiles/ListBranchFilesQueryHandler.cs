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
    private readonly IUserRepository _userRepository;
    
    public ListBranchFilesQueryHandler(
        IGitService gitService, 
        IBranchRepository branchRepository, 
        IRepositoryRepository repositoryRepository, 
        IUserRepository userRepository)
    {
        _gitService = gitService;
        _branchRepository = branchRepository;
        _repositoryRepository = repositoryRepository;
        _userRepository = userRepository;
    }
    
    
    public async Task<List<ContributionFile>> Handle(ListBranchFilesQuery request, CancellationToken cancellationToken)
    {
        var branch = await _branchRepository.FindById(request.BranchId);
        Branch.ThrowIfDoesntExist(branch); 
        
        var owner = await _repositoryRepository.FindRepositoryOwner(branch!.RepositoryId);
        User.ThrowIfDoesntExist(owner);
        
        return await _gitService.ListFolderContent(owner!, branch, request.Folder);
    }
}