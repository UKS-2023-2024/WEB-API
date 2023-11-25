using Application.Shared;
using Domain.Branches;
using Domain.Branches.Interfaces;
using Domain.Repositories.Exceptions;
using Domain.Repositories.Interfaces;
using Domain.Repositories;
using Domain.Branches.Exceptions;
using Application.Branches.Commands.Restore;

namespace Application.Branches.Commands.Delete;

public class RestoreBranchCommandHandler : ICommandHandler<DeleteBranchCommand, Branch>
{
    private readonly IBranchRepository _branchRepository;

    public RestoreBranchCommandHandler(IBranchRepository branchRepository)
    {
        _branchRepository = branchRepository;
    }

    public async Task<Branch> Handle(RestoreBranchCommand request, CancellationToken cancellationToken)
    {
        Branch? branch = _branchRepository.Find(request.BranchId);
        if (branch is null)
            throw new BranchNotFoundException();
        branch.Restore();
        _branchRepository.Update(branch);
        return branch;
    }
}