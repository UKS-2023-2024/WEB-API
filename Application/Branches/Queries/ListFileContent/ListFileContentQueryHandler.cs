using Application.Shared;
using Domain.Auth;
using Domain.Auth.Interfaces;
using Domain.Branches;
using Domain.Branches.Interfaces;
using Domain.Repositories.Interfaces;
using Domain.Shared.Interfaces;

namespace Application.Branches.Queries.ListFileContent;

public class ListFileContentQueryHandler: IQueryHandler<ListFileContentQuery, FileContent>
{
    private readonly IGitService _gitService;
    private readonly IBranchRepository _branchRepository;
    private readonly IRepositoryRepository _repositoryRepository;
    private readonly IUserRepository _userRepository;
    
    public ListFileContentQueryHandler(
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
    
    public async Task<FileContent> Handle(ListFileContentQuery request, CancellationToken cancellationToken)
    {
        var branch = await _branchRepository.FindById(request.BranchId);
        Branch.ThrowIfDoesntExist(branch); 
        
        var owner = await _repositoryRepository.FindRepositoryOwner(branch!.RepositoryId);
        User.ThrowIfDoesntExist(owner);
        
        return await _gitService.ListFileContent(owner!, branch, request.Path);
    }
}