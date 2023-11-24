using Application.Milestones.Commands.Create;
using Application.Shared;
using Domain.Branches;
using Domain.Branches.Exceptions;
using Domain.Branches.Interfaces;
namespace Application.Branches.Commands.Update;

public class MakeBranchDefaultCommandHandler : ICommandHandler<MakeBranchDefaultCommand, Branch>
{
    private readonly IBranchRepository _branchRepository;

    public MakeBranchDefaultCommandHandler(IBranchRepository branchRepository)
    {
        _branchRepository = branchRepository;
    }

    public async Task<Branch> Handle(MakeBranchDefaultCommand request, CancellationToken cancellationToken)
    {
        Branch? branch = _branchRepository.Find(request.BranchId);
        if (branch is null)
            throw new BranchNotFoundException();

        branch.UpdateDefault();
        _branchRepository.Update(branch);

        Branch? oldDefaultBranch = await _branchRepository.FindByRepositoryIdAndIsDefault(branch.RepositoryId, true);
        if (oldDefaultBranch is not null)
        {
            oldDefaultBranch.UpdateDefault();
            _branchRepository.Update(oldDefaultBranch);
        }

        return branch;
    }
    
}