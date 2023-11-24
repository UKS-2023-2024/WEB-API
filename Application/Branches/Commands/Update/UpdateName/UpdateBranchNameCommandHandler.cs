using Application.Milestones.Commands.Create;
using Application.Shared;
using Domain.Branches;
using Domain.Branches.Exceptions;
using Domain.Branches.Interfaces;
using Domain.Milestones;
using Domain.Milestones.Exceptions;
using Domain.Milestones.Interfaces;
using Domain.Repositories;
using Domain.Repositories.Exceptions;
namespace Application.Branches.Commands.Update;

public class UpdateBranchNameCommandHandler : ICommandHandler<UpdateBranchNameCommand, Branch>
{
    private readonly IBranchRepository _branchRepository;

    public UpdateBranchNameCommandHandler(IBranchRepository branchRepository)
    {
        _branchRepository = branchRepository;
    }

    public async Task<Branch> Handle(UpdateBranchNameCommand request, CancellationToken cancellationToken)
    {
        Branch? branch = _branchRepository.Find(request.BranchId);
        if (branch is null)
            throw new BranchNotFoundException();

        Branch? existingBranch = await _branchRepository.FindByNameAndRepositoryId(request.Name, branch.RepositoryId);
        if (existingBranch is not null && existingBranch.Id != branch.Id)
            throw new BranchWithThisNameExistsException();

        branch.Update(request.Name);
        _branchRepository.Update(branch);
        return branch;
    }
    
}