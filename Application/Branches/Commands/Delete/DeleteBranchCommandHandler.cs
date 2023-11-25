using Application.Shared;
using Domain.Branches;
using Domain.Branches.Interfaces;
using Domain.Repositories.Exceptions;
using Domain.Repositories.Interfaces;
using Domain.Repositories;
using Domain.Branches.Exceptions;

namespace Application.Branches.Commands.Delete;

public class DeleteBranchCommandHandler : ICommandHandler<DeleteBranchCommand, Branch>
{
    private readonly IBranchRepository _branchRepository;

    public DeleteBranchCommandHandler(IBranchRepository branchRepository)
    {
        _branchRepository = branchRepository;
    }

    public async Task<Branch> Handle(DeleteBranchCommand request, CancellationToken cancellationToken)
    {
        Branch? branch = _branchRepository.Find(request.BranchId);
        if (branch is null)
            throw new BranchNotFoundException();
        branch.Delete();
        _branchRepository.Update(branch);
        return branch;
    }
}