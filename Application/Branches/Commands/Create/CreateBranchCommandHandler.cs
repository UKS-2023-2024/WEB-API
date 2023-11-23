using Application.Shared;
using Domain.Branches;
using Domain.Branches.Interfaces;

namespace Application.Branches.Commands.Create;

public class CreateBranchCommandHandler : ICommandHandler<CreateBranchCommand, Guid>
{
    private readonly IBranchRepository _branchRepository;

    public CreateBranchCommandHandler(IBranchRepository branchRepository)
    {
        _branchRepository = branchRepository;
    }

    public async Task<Guid> Handle(CreateBranchCommand request, CancellationToken cancellationToken)
    {

        Branch branch = Branch.Create(request.Name, request.RepositoryId, request.IsDefault);
        Branch createdBranch = await _branchRepository.Create(branch);
        return createdBranch.Id;
    }
}