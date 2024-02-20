using Application.Shared;
using Domain.Branches;
using Domain.Branches.Interfaces;
using Domain.Repositories.Exceptions;
using Domain.Repositories.Interfaces;
using Domain.Repositories;
using Domain.Branches.Exceptions;
using Domain.Shared.Interfaces;

namespace Application.Branches.Commands.Create;

public class CreateBranchCommandHandler : ICommandHandler<CreateBranchCommand, Guid>
{
    private readonly IBranchRepository _branchRepository;
    private readonly IGitService _gitService;
    private readonly IRepositoryRepository _repositoryRepository;

    public CreateBranchCommandHandler(IBranchRepository branchRepository, IGitService gitService,
        IRepositoryRepository repositoryRepository)
    {
        _branchRepository = branchRepository;
        _gitService = gitService;
        _repositoryRepository = repositoryRepository;
    }

    public async Task<Guid> Handle(CreateBranchCommand request, CancellationToken cancellationToken)
    {
        var existingBranch =
        await _branchRepository.FindByNameAndRepositoryId(request.Name, request.RepositoryId);
        if (existingBranch is not null)
            throw new BranchWithThisNameExistsException();
        var createFromBranch =
            await _branchRepository.FindByNameAndRepositoryId(request.CreateFromBranch, request.RepositoryId);
        if (createFromBranch is null)
            throw new BranchNotFoundException();
        var branch = Branch.Create(request.Name, request.RepositoryId, request.IsDefault, request.OwnerId,createFromBranch.Name);
        
        var createdBranch = await _branchRepository.Create(branch);

        var repository = _repositoryRepository.Find(request.RepositoryId);
        await _gitService.CreateBranch(repository!,request.Name,request.CreateFromBranch);
        
        return createdBranch.Id;
    }
}