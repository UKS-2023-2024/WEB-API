using Application.Milestones.Commands.Create;
using Application.Shared;
using Domain.Branches;
using Domain.Branches.Exceptions;
using Domain.Branches.Interfaces;
using Domain.Repositories;
using Domain.Repositories.Interfaces;
using Domain.Shared.Interfaces;

namespace Application.Branches.Commands.Update;

public class MakeBranchDefaultCommandHandler : ICommandHandler<MakeBranchDefaultCommand, Branch>
{
    private readonly IBranchRepository _branchRepository;
    private readonly IRepositoryRepository _repositoryRepository;
    private readonly IGitService _gitService;

    public MakeBranchDefaultCommandHandler(IBranchRepository branchRepository, IRepositoryRepository repositoryRepository, IGitService gitService)
    {
        _branchRepository = branchRepository;
        _repositoryRepository = repositoryRepository;
        _gitService = gitService;
    }

    public async Task<Branch> Handle(MakeBranchDefaultCommand request, CancellationToken cancellationToken)
    {
        Branch? branch = _branchRepository.Find(request.BranchId);
        if (branch is null)
            throw new BranchNotFoundException();
        if (branch.IsDefault)
            throw new BranchIsAlreadyDefaultException();

        Branch? oldDefaultBranch = await _branchRepository.FindByRepositoryIdAndIsDefault(branch.RepositoryId, true);
        if (oldDefaultBranch is not null)
        {
            oldDefaultBranch.ChangeDefault(false);
            _branchRepository.Update(oldDefaultBranch);
        }

        branch.ChangeDefault(true);
        _branchRepository.Update(branch);

        var repository = _repositoryRepository.Find(branch.RepositoryId);
        Repository.ThrowIfDoesntExist(repository);
        
        await _gitService.UpdateRepository(repository!,branch!.OriginalName,repository!.Name);

        return branch;
    }
    
}