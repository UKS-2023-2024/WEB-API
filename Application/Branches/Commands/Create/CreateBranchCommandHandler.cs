﻿using Application.Shared;
using Domain.Branches;
using Domain.Branches.Interfaces;
using Domain.Repositories.Exceptions;
using Domain.Repositories.Interfaces;
using Domain.Repositories;
using Domain.Branches.Exceptions;

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
        Branch? existingBranch =
        await _branchRepository.FindByNameAndRepositoryId(request.Name, request.RepositoryId);
        if (existingBranch is not null)
            throw new BranchWithThisNameExistsException();

        Branch branch = Branch.Create(request.Name, request.RepositoryId, request.IsDefault, request.OwnerId);
        Branch createdBranch = await _branchRepository.Create(branch);
        return createdBranch.Id;
    }
}