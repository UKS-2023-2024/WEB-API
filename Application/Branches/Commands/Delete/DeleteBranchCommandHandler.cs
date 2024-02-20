using Application.Shared;
using Domain.Auth.Interfaces;
using Domain.Branches;
using Domain.Branches.Interfaces;
using Domain.Repositories.Exceptions;
using Domain.Repositories.Interfaces;
using Domain.Repositories;
using Domain.Branches.Exceptions;
using Domain.Shared.Interfaces;

namespace Application.Branches.Commands.Delete;

public class DeleteBranchCommandHandler : ICommandHandler<DeleteBranchCommand, Branch>
{
    private readonly IBranchRepository _branchRepository;
    private readonly IGitService _gitService;
    private readonly IRepositoryRepository _repositoryRepository;
    public DeleteBranchCommandHandler(IBranchRepository branchRepository, IGitService gitService, IRepositoryRepository repositoryRepository)
    {
        _branchRepository = branchRepository;
        _gitService = gitService;
        _repositoryRepository = repositoryRepository;
    }

    public async Task<Branch> Handle(DeleteBranchCommand request, CancellationToken cancellationToken)
    {
        Branch? branch = await _branchRepository.FindById(request.BranchId);
        if (branch is null)
            throw new BranchNotFoundException();
        branch.Delete();
        _branchRepository.Update(branch);
        
        
        // Additional git setup
        var repository = _repositoryRepository.Find(branch.RepositoryId);
        await _gitService.DeleteBranch(repository, branch);
        
        
        return branch;
    }
}